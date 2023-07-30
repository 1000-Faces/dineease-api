using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using webapi.Models;
namespace webapi.Endpoints;

public static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Authentication").WithTags(nameof(Authentication));

        group.MapGet("/", async (MainDatabaseContext db) =>
        {
            return await db.Authentication.ToListAsync();
        })
        .WithName("GetAllAuthentications")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Authentication>, NotFound>> (int userid, MainDatabaseContext db) =>
        {
            return await db.Authentication.AsNoTracking()
                .FirstOrDefaultAsync(model => model.UserId == userid)
                is Authentication model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetAuthenticationById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int userid, Authentication authentication, MainDatabaseContext db) =>
        {
            var affected = await db.Authentication
                .Where(model => model.UserId == userid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.UserId, authentication.UserId)
                  .SetProperty(m => m.Password, authentication.Password)
                  .SetProperty(m => m.Role, authentication.Role)
                  .SetProperty(m => m.LastLogged, authentication.LastLogged)
                  .SetProperty(m => m.LastUpdated, authentication.LastUpdated)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateAuthentication")
        .WithOpenApi();

        group.MapPost("/", async (Authentication authentication, MainDatabaseContext db) =>
        {
            db.Authentication.Add(authentication);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Authentication/{authentication.UserId}",authentication);
        })
        .WithName("CreateAuthentication")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int userid, MainDatabaseContext db) =>
        {
            var affected = await db.Authentication
                .Where(model => model.UserId == userid)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteAuthentication")
        .WithOpenApi();
    }
}
