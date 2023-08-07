using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using webapi.Models;
namespace webapi.Endpoints;

public static class OrdersEndpoints
{
    public static void MapOrdersEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Orders").WithTags(nameof(Orders));

        group.MapGet("/", async (MainDatabaseContext db) =>
        {
            return await db.Orders.ToListAsync();
        })
        .WithName("GetAllOrders")
        .WithOpenApi();

        group.MapGet("/pending", async (MainDatabaseContext db) =>
        {
            var pendingorder = await db.Orders
                .Where(r => r.OrderStatus == "pending")
                .ToListAsync();

            return pendingorder;
        })
        .WithName("getpendingorders")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Orders>, NotFound>> (string orderid, MainDatabaseContext db) =>
        {
            return await db.Orders.AsNoTracking()
                .FirstOrDefaultAsync(model => model.OrderId == orderid)
                is Orders model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetOrdersById")
        .WithOpenApi();

        //group.MapGet("/get", async Task<Results<Ok<User>, NotFound, BadRequest<string>>> (Guid? id, string? email, MainDatabaseContext db) =>
        group.MapPut("/put", async Task<Results<Ok, NotFound , BadRequest<string>>> (string? orderid, string? status, Orders? orders, MainDatabaseContext db) =>
        {
            var affected = await db.Orders
                .Where(model => model.OrderId == orderid)
                .ExecuteUpdateAsync(setters => setters
                  //.SetProperty(m => m.OrderId, orders.OrderId)
                  //.SetProperty(m => m.ReservationId, orders.ReservationId)
                  //.SetProperty(m => m.Total, orders.Total)
                  //.SetProperty(m => m.Discount, orders.Discount)
                  //.SetProperty(m => m.Price, orders.Price)
                  .SetProperty(m => m.OrderStatus, status)
                  //.SetProperty(m => m.OrderStatus, orders.OrderStatus)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateOrders")
        .WithOpenApi();

        group.MapPost("/", async (Orders orders, MainDatabaseContext db) =>
        {
            db.Orders.Add(orders);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Orders/{orders.OrderId}",orders);
        })
        .WithName("CreateOrders")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (string orderid, MainDatabaseContext db) =>
        {
            var affected = await db.Orders
                .Where(model => model.OrderId == orderid)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteOrders")
        .WithOpenApi();
    }
}
