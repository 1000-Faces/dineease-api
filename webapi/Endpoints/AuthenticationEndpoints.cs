using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using webapi.Models;
using webapi.DataModels;
using webapi.Services;
using Microsoft.OpenApi.Writers;

namespace webapi.Endpoints;

public static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/auth").WithTags(nameof(Authentication));

        // Check if account exists by email
        group.MapGet("/{email}", async Task<Results<Ok<User>, NotFound>> (string email, MainDatabaseContext db) =>
        {
            return await db.User.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Email == email)
                is User model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("CheckAccountExists")
        .WithOpenApi();


        // Authenticate user by email and password
        group.MapPost("/", async Task<Results<Ok, NotFound>> (LoginData data, MainDatabaseContext db) =>
        {
            var user = await db.User
                .Include(u => u.Authentication)
                .FirstOrDefaultAsync(u => u.Email == data.Email);

            if (user == null || !PasswordHasher.Verify(data.Password, user.Authentication?.Password, user.Authentication?.Salt))
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok();
        })
        .WithName("AuthenticateUser")
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

        group.MapPost("/register", async (Authentication authentication, MainDatabaseContext db) =>
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
