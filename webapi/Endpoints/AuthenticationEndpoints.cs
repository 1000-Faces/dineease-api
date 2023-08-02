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
        group.MapPost("/", async Task<Results<Accepted<User>, UnauthorizedHttpResult>> (LoginData data, MainDatabaseContext db) =>
        {
            // get user by email with authentication data
            var user = await db.User
                .Include(u => u.Authentication)
                .FirstOrDefaultAsync(u => u.Email == data.Email);

            // user can be null here. If not, verify password
            if (user == null || !PasswordHasher.Verify(data.Password, user.Authentication?.Password, user.Authentication?.Salt))
            {
                return TypedResults.Unauthorized();
            }

            // Update the last logged date
            await db.Authentication
                .Where(model => model.UserId == user.Id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.LastLogged, DateTime.Now)
                );

            return TypedResults.Accepted($"/api/auth/{user.Id}", user);
        })
        .WithName("AuthenticateUser")
        .WithOpenApi();
    }
}
