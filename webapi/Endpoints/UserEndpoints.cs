using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using webapi.Models;
using Microsoft.AspNetCore.Mvc;
using webapi.Services;

namespace webapi.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/user").WithTags(nameof(User));

        group.MapGet("/", async (MainDatabaseContext db) =>
        {
            return await db.User.ToListAsync();
        })
        .WithName("GetAllUsers")
        .WithOpenApi();

        group.MapGet("/get", async Task<Results<Ok<User>, NotFound, BadRequest<string>>> (int? id, string? email, MainDatabaseContext db) =>
        {
            if (id.HasValue && id.Value > 0)
            {
                // If "id" parameter is provided and valid, get user by id
                return await db.User.AsNoTracking()
                    .FirstOrDefaultAsync(model => model.Id == id.Value)
                    is User model
                        ? TypedResults.Ok(model)
                        : TypedResults.NotFound();
            }
            else if (!string.IsNullOrEmpty(email))
            {
                // If "email" parameter is provided, get user by email
                return await db.User.AsNoTracking()
                    .FirstOrDefaultAsync(model => model.Email == email)
                    is User model
                        ? TypedResults.Ok(model)
                        : TypedResults.NotFound();
            }
            else
            {
                // Neither "id" nor "email" parameter is provided
                // Return a bad request response or handle it based on your specific use case
                return TypedResults.BadRequest("Invalid request. Either 'id' or 'email' parameter is required.");
            }
        })
        .WithName("GetUserByEmailOrId")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, User user, MainDatabaseContext db) =>
        {
            var affected = await db.User
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, user.Id)
                  .SetProperty(m => m.Username, user.Username)
                  .SetProperty(m => m.Name, user.Name)
                  .SetProperty(m => m.Address, user.Address)
                  .SetProperty(m => m.Phone, user.Phone)
                  .SetProperty(m => m.Email, user.Email)
                  .SetProperty(m => m.UserImage, user.UserImage)
                  .SetProperty(m => m.UserImageType, user.UserImageType)
                  .SetProperty(m => m.CreatedDate, user.CreatedDate)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateUser")
        .WithOpenApi();

        group.MapPost("/", async Task<Results<Created<User>, BadRequest<string>>> (User user, MainDatabaseContext db) =>
        {
            // Check if user already exists
            if (await db.User.AnyAsync(model => model.Email == user.Email))
            {
                return TypedResults.BadRequest("User already exists.");
            }

            // Check if user has authentication data
            if (user.Authentication == null)
            {
                return TypedResults.BadRequest("User authentication data is required.");
            }

            // secure the password
            var hashedPassword = PasswordHasher.Hash(user.Authentication.Password);
            user.Authentication.Password = hashedPassword.Item1;
            user.Authentication.Salt = hashedPassword.Item2;

            var result = user;
            result.Authentication = null;

            db.User.Add(user);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/User/{user.Id}", result);
        })
        .WithName("CreateUser")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, MainDatabaseContext db) =>
        {
            var affected = await db.User
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteUser")
        .WithOpenApi();
    }
}
