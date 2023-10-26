using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using webapi.Models;
namespace webapi.Endpoints;

public static class MealEndpoints
{
    public static void MapMealEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Meal").WithTags(nameof(Meal));

        group.MapGet("/", async (MainDatabaseContext db) =>
        {
            return await db.Meal.ToListAsync();
        })
        .WithName("GetAllMeals")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Meal>, NotFound>> (Guid mealid, MainDatabaseContext db) =>
        {
            return await db.Meal.AsNoTracking()
                .FirstOrDefaultAsync(model => model.MealId == mealid)
                is Meal model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetMealById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (Guid mealid, Meal meal, MainDatabaseContext db) =>
        {
            var affected = await db.Meal
                .Where(model => model.MealId == mealid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.MealId, meal.MealId)
                  .SetProperty(m => m.MealName, meal.MealName)
                  .SetProperty(m => m.Discription, meal.Discription)
                  .SetProperty(m => m.Price, meal.Price)
                  .SetProperty(m => m.Custom, meal.Custom)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateMeal")
        .WithOpenApi();

        group.MapPost("/", async (Meal meal, MainDatabaseContext db) =>
        {
            meal.MealId = Guid.NewGuid();
            db.Meal.Add(meal);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Meal/{meal.MealId}",meal);
        })
        .WithName("CreateMeal")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (Guid mealid, MainDatabaseContext db) =>
        {
            var affected = await db.Meal
                .Where(model => model.MealId == mealid)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteMeal")
        .WithOpenApi();
    }
}
