using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using webapi.Models;
namespace webapi.Endpoints;

public static class PromotionEndpoints
{
    public static void MapPromotionEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Promotion").WithTags(nameof(Promotion));

        group.MapGet("/", async (MainDatabaseContext db) =>
        {
            return await db.Promotion.ToListAsync();
        })
        .WithName("GetAllPromotions")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Promotion>, NotFound>> (Guid promotionid, MainDatabaseContext db) =>
        {
            return await db.Promotion.AsNoTracking()
                .FirstOrDefaultAsync(model => model.PromotionId == promotionid)
                is Promotion model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetPromotionById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (Guid promotionid, Promotion promotion, MainDatabaseContext db) =>
        {
            var affected = await db.Promotion
                .Where(model => model.PromotionId == promotionid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.PromotionId, promotion.PromotionId)
                  .SetProperty(m => m.Discount, promotion.Discount)
                  .SetProperty(m => m.Description, promotion.Description)
                  .SetProperty(m => m.Deadline, promotion.Deadline)
                  .SetProperty(m => m.Status, promotion.Status)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdatePromotion")
        .WithOpenApi();

        group.MapPost("/", async (Promotion promotion, MainDatabaseContext db) =>
        {
            db.Promotion.Add(promotion);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Promotion/{promotion.PromotionId}",promotion);
        })
        .WithName("CreatePromotion")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (Guid promotionid, MainDatabaseContext db) =>
        {
            var affected = await db.Promotion
                .Where(model => model.PromotionId == promotionid)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeletePromotion")
        .WithOpenApi();
    }
}
