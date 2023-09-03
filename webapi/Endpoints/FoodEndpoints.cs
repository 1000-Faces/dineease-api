using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using webapi.Models;
namespace webapi.Endpoints;

public static class FoodEndpoints
{
    public static void MapFoodEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Food").WithTags(nameof(Food));

        group.MapGet("/", async (MainDatabaseContext db) =>
        {
            return await db.Food.ToListAsync();
        })
        .WithName("GetAllFoods")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Food>, NotFound>> (Guid foodid, MainDatabaseContext db) =>
        {
            return await db.Food.AsNoTracking()
                .FirstOrDefaultAsync(model => model.FoodId == foodid)
                is Food model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetFoodById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (Guid foodid, Food food, MainDatabaseContext db) =>
        {
            var affected = await db.Food
                .Where(model => model.FoodId == foodid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.FoodName, food.FoodName)
                  .SetProperty(m => m.Description, food.Description)
                  .SetProperty(m => m.CategoryId, food.CategoryId)
                  .SetProperty(m => m.FoodType, food.FoodType)
                  .SetProperty(m => m.Availability, food.Availability)
                  .SetProperty(m => m.Price, food.Price)
                  .SetProperty(m => m.FoodId, food.FoodId)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateFood")
        .WithOpenApi();

        group.MapPost("/", async (Food food, MainDatabaseContext db) =>
        {
            food.FoodId = Guid.NewGuid();
            db.Food.Add(food);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Food/{food.FoodId}",food);
        })
        .WithName("CreateFood")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (Guid foodid, MainDatabaseContext db) =>
        {
            var affected = await db.Food
                .Where(model => model.FoodId == foodid)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteFood")
        .WithOpenApi();
    }
}
