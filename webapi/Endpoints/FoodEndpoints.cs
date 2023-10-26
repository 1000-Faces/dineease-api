using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using webapi.Models;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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

        //group.MapPost("/", async Task<Results<Ok, Created<Food>, NotFound<string>>> (IFormFile imageFile, [FromForm] Food food, MainDatabaseContext db, IWebHostEnvironment environment) =>
        group.MapPost("/", async Task<Results<Ok, Created<Food>, NotFound<string>>> (Food food, MainDatabaseContext db, IWebHostEnvironment environment) =>
        {
            try
            {
                //return TypedResults.Ok();
                food.FoodId = Guid.NewGuid();

                if (food.ImageFile != null && food.ImageFile.Length > 0)
                {
                    // Retrieve the uploaded image (IFormFile)
                    string imgFileName = Path.GetFileNameWithoutExtension(food.ImageFile.FileName);
                    string imgExtention = Path.GetExtension(food.ImageFile.FileName);

                    imgFileName = imgFileName + DateTime.Now.ToString("yymmssfff") + imgExtention;
                    food.FoodImg = "~/Images/" + imgFileName;

                    // saving image in the server Images folder
                    string imgFilePath = Path.Combine(environment.WebRootPath, "Images", imgFileName);

                    using (var stream = new FileStream(imgFilePath, FileMode.Create))
                    {
                        await food.ImageFile.CopyToAsync(stream);
                    }
                }

                db.Food.Add(food);
                await db.SaveChangesAsync();
                return TypedResults.Created($"/api/Food/{food.FoodId}", food);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.ToString());

                // Return an error response
                return TypedResults.NotFound("An error occurred while processing your request.");
            }

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
