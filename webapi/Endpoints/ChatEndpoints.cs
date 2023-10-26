using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using webapi.Models;
namespace webapi.Endpoints;

public static class ChatEndpoints
{
    public static void MapChatEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Chat").WithTags(nameof(Chat));

        group.MapGet("/", async (MainDatabaseContext db) =>
        {
            return await db.Chat.ToListAsync();
        })
        .WithName("GetAllChats")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Chat>, NotFound>> (Guid messageid, MainDatabaseContext db) =>
        {
            return await db.Chat.AsNoTracking()
                .FirstOrDefaultAsync(model => model.MessageId == messageid)
                is Chat model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetChatById")
        .WithOpenApi();

        group.MapGet("/user_chat", async Task<Results<Ok<List<Chat>>, NotFound>> (Guid userId, MainDatabaseContext db) =>
        {
            var chatMessages = await db.Chat.AsNoTracking()
                .Where(model => model.SenderId == userId || model.ReceiverId == userId)
                .ToListAsync();

            if (chatMessages.Any())
            {
                return TypedResults.Ok(chatMessages);
            }
            else
            {
                return TypedResults.NotFound();
            }
        })
.       WithName("GetChatMessagesByUserId")
.       WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (Guid messageid, Chat chat, MainDatabaseContext db) =>
        {
            var affected = await db.Chat
                .Where(model => model.MessageId == messageid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.MessageId, chat.MessageId)
                  .SetProperty(m => m.SenderId, chat.SenderId)
                  .SetProperty(m => m.ReceiverId, chat.ReceiverId)
                  .SetProperty(m => m.Timestamp, chat.Timestamp)
                  .SetProperty(m => m.DateTime, chat.DateTime)
                  .SetProperty(m => m.Message, chat.Message)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateChat")
        .WithOpenApi();

        group.MapPost("/", async Task<Results<Created<Object>, BadRequest>> (Guid? senderId, Guid? receiverId,Chat chat, MainDatabaseContext db) =>
        {
            chat.MessageId = Guid.NewGuid();
            chat.SenderId = senderId;
            chat.ReceiverId = receiverId;
            chat.DateTime = DateTime.Now;
            // Check if the message is not empty
            if (!string.IsNullOrWhiteSpace(chat.Message))
            {
                db.Chat.Add(chat);
                await db.SaveChangesAsync();
                return TypedResults.Created<Object>($"/api/Chat/{chat.MessageId}", chat);
            }
            else
            {
                // Handle the case where the message is empty
                return TypedResults.BadRequest();
            }
            //db.Chat.Add(chat);
            //await db.SaveChangesAsync();
            //return TypedResults.Created($"/api/Chat/{chat.MessageId}",chat);
        })
        .WithName("CreateChat")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (Guid messageid, MainDatabaseContext db) =>
        {
            var affected = await db.Chat
                .Where(model => model.MessageId == messageid)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteChat")
        .WithOpenApi();
    }
}
