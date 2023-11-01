using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using webapi.Models;
using Microsoft.AspNetCore.Http;
using webapi.Services;
using FirebaseAdmin.Auth;


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

        //List<string> mealNames, 
        group.MapPost("/", async Task<Results<Ok<Guid>, NotFound<string>, BadRequest<string>>> (List<string> food_ids, Guid? userID, MainDatabaseContext db, IHttpContextAccessor httpContextAccessor) =>
        {

            // Retrieve session token from HttpContext
            var context = httpContextAccessor.HttpContext;
            var sessionToken = context?.Session.GetString("_UserToken");

            if (string.IsNullOrEmpty(sessionToken))
            {
                // Session token not found, handle the case (e.g., return Unauthorized)
                return TypedResults.NotFound("Session token not found.");
            }

            // Verify the ID token using Firebase Admin SDK
            //var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(sessionToken);

            // Get user's email from the decoded token
            //var userEmail = decodedToken.Claims["email"].ToString();

            //return TypedResults.NotFound(userEmail);


            var currentReservationId = await db.Reservation
                .Where(r => r.CustomerId == userID &&
                r.Status == "arrived" )
                .Select(r => r.ReservationId)
                .FirstOrDefaultAsync();

            if(currentReservationId == Guid.Empty)
            {
                return TypedResults.NotFound("Reservation not found");
            }


            var currentOrder = await db.Orders
                    .Where(o => o.ReservationId == currentReservationId && o.OrderStatus.Trim() == "pending")
                    .Select (o => o.OrderId)
                    .FirstOrDefaultAsync();

            if(currentOrder == Guid.Empty)
            {
                return TypedResults.BadRequest("Cannot change order now");
            }

            //adding new meal
            var meal = new Meal();
            meal.MealId = Guid.NewGuid();
            meal.Custom = true;
            meal.MealName = "custom meal";
            

            //Adding meal to the order
            var orderMeal = new OrderMeal();
            orderMeal.OrderId = currentOrder;
            orderMeal.MealId = meal.MealId;
            orderMeal.Quantity = 1;

            db.Meal.Add(meal);
            db.OrderMeal.Add(orderMeal);
            db.SaveChanges();

            var foodIdCounts = new Dictionary<Guid, int>();

            foreach (var foodIdString in food_ids)
            {
                if (Guid.TryParse(foodIdString, out Guid foodId))
                {
                    // If foodId is already in the dictionary, increment the count
                    if (foodIdCounts.ContainsKey(foodId))
                    {
                        foodIdCounts[foodId]++;
                        var existingMealFood = db.MealFoods
                                    .FirstOrDefault(mf => mf.MealId == meal.MealId && mf.FoodId == foodId);

                        if (existingMealFood != null)
                        {
                            existingMealFood.Quantity = foodIdCounts[foodId];
                            db.MealFoods.Update(existingMealFood);
                        }
                        db.SaveChanges();
                    }
                    else
                    {
                        // If foodId is not in the dictionary, add it with count 1
                        foodIdCounts[foodId] = 1;
                        var mealFood = new MealFoods
                        {
                            MealId = meal.MealId,
                            FoodId = foodId,
                            Quantity = foodIdCounts[foodId] // Set the appropriate quantity here
                        };
                        db.MealFoods.Add(mealFood);
                        db.SaveChanges();
                    }     
                }
                else
                {
                    // Handle invalid Guid format in food_ids if necessary
                }
            }



            await db.SaveChangesAsync();
            return TypedResults.Ok(currentOrder);
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
