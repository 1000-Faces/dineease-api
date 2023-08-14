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
        //group.MapGet("/{email}", async Task<Results<Ok<User>, NotFound>> (string email, MainDatabaseContext db) =>
        //{
        //    return await db.User.AsNoTracking()
        //        .FirstOrDefaultAsync(model => model.Email == email)
        //        is User model
        //            ? TypedResults.Ok(model)
        //            : TypedResults.NotFound();
        //})
        //.WithName("CheckAccountExists")
        //.WithOpenApi();

        group.MapGet("/{email_id}", async Task<Results<Ok<Object>, NotFound>> (Guid? id,string? email, MainDatabaseContext db) =>
        {
            if (id.HasValue || !string.IsNullOrEmpty(email))
            {
                User? userModel = null;
                if (id.HasValue)
                {
                    userModel = await db.User.AsNoTracking()
                    .FirstOrDefaultAsync(model => model.Id == id);
                }
                else if (!string.IsNullOrEmpty(email))
                {
                    userModel = await db.User.AsNoTracking()
                    .FirstOrDefaultAsync(model => model.Email == email);
                }

                if (userModel != null)
                {
                    var authentication = await db.Authentication.AsNoTracking()
                        .FirstOrDefaultAsync(auth => auth.UserId == userModel.Id);

                    if (authentication != null)
                    {
                        var response = new
                        {
                            User = userModel,
                            Role = authentication.Role
                        };

                        return TypedResults.Ok<Object>(response);
                    }
                }
            }

            return TypedResults.NotFound();
        })
        .WithName("CheckAccountExists")
        .WithOpenApi();


        // Authenticate user by email and password
        group.MapPost("/", async Task<Results<Accepted<Guid>, UnauthorizedHttpResult>> (LoginData data, MainDatabaseContext db) =>
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

            return TypedResults.Accepted($"/api/auth/", user.Id);
        })
        .WithName("AuthenticateUser")
        .WithOpenApi();
    }
}
