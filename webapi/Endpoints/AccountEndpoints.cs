using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using webapi.Models;
namespace webapi.Endpoints;

public static class AccountEndpoints
{
    public static void MapAccountEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Account").WithTags(nameof(Account));

        group.MapGet("/", async (MainDatabaseContext db) =>
        {
            return await db.Account.ToListAsync();
        })
        .WithName("GetAllAccounts")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Account>, NotFound>> (string userid, MainDatabaseContext db) =>
        {
            return await db.Account.AsNoTracking()
                .FirstOrDefaultAsync(model => model.UserId == userid)
                is Account model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetAccountById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (string userid, Account account, MainDatabaseContext db) =>
        {
            var affected = await db.Account
                .Where(model => model.UserId == userid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.UserId, account.UserId)
                  .SetProperty(m => m.Password, account.Password)
                  .SetProperty(m => m.Role, account.Role)
                  .SetProperty(m => m.CreatedDate, account.CreatedDate)
                  .SetProperty(m => m.LastUpdated, account.LastUpdated)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateAccount")
        .WithOpenApi();

        group.MapPost("/", async (Account account, MainDatabaseContext db) =>
        {
            db.Account.Add(account);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Account/{account.UserId}",account);
        })
        .WithName("CreateAccount")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (string userid, MainDatabaseContext db) =>
        {
            var affected = await db.Account
                .Where(model => model.UserId == userid)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteAccount")
        .WithOpenApi();
    }
}
