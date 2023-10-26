using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using webapi.Models;
namespace webapi.Endpoints;

public static class CheckoutEndpoints
{
    public static void MapCheckoutEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Checkout").WithTags(nameof(Checkout));

        group.MapGet("/", async (MainDatabaseContext db) =>
        {
            return await db.Checkout.ToListAsync();
        })
        .WithName("GetAllCheckouts")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Checkout>, NotFound>> (Guid orderid, MainDatabaseContext db) =>
        {
            return await db.Checkout.AsNoTracking()
                .FirstOrDefaultAsync(model => model.OrderId == orderid)
                is Checkout model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetCheckoutById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (Guid orderid, Checkout checkout, MainDatabaseContext db) =>
        {
            var affected = await db.Checkout
                .Where(model => model.OrderId == orderid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.OrderId, checkout.OrderId)
                  .SetProperty(m => m.StaffId, checkout.StaffId)
                  .SetProperty(m => m.PaymentMethod, checkout.PaymentMethod)
                  .SetProperty(m => m.Amount, checkout.Amount)
                  .SetProperty(m => m.CheckoutTime, checkout.CheckoutTime)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateCheckout")
        .WithOpenApi();

        group.MapPost("/", async (Checkout checkout, MainDatabaseContext db) =>
        {
            db.Checkout.Add(checkout);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Checkout/{checkout.OrderId}",checkout);
        })
        .WithName("CreateCheckout")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (Guid orderid, MainDatabaseContext db) =>
        {
            var affected = await db.Checkout
                .Where(model => model.OrderId == orderid)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteCheckout")
        .WithOpenApi();
    }
}
