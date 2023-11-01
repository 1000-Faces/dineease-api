using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using webapi.Models;
using Stripe.Checkout;

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

        group.MapGet("/checkoutOrder/{id}", async Task<Results<Ok<string>, BadRequest<string>>> (Guid orderid, MainDatabaseContext db) =>
        {

            var order = await db.Orders.AsNoTracking()
                .FirstOrDefaultAsync(model => model.OrderId == orderid);

            if (order != null && order.OrderStatus.Trim() == "pai")
            {
                return TypedResults.BadRequest("The Order has been paid");
            }
            // 4d4bc2a8-fabe-417e-9654-025c5b4961b1
            var affected = await db.Orders
                    .Where(model => model.OrderId == orderid)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(m => m.OrderStatus, "paid")
                    );

            await db.SaveChangesAsync();

            // Stripe payments start
            var price = 0;
            if (order != null)
            {
                price = (int)order.Total * 100;
            }

            var domain = "https://localhost:7251/";

            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"confirmcheckout",
                CancelUrl = domain + $"cancelcheckout",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };


            var sessionListItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions()
                {
                    UnitAmount = price,
                    Currency = "lkr",
                    ProductData = new SessionLineItemPriceDataProductDataOptions()
                    {
                        Name = "Order" + orderid
                    }
                },
                Quantity = 1
            };
            options.LineItems.Add(sessionListItem);

            var service = new SessionService();
            Session session = service.Create(options);

            //// Perform the redirect by setting the "Location" header
            var redirectUrl = session.Url;
            return TypedResults.Ok(redirectUrl);

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
