using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using webapi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml;
using Microsoft.Data.SqlClient;
using static NuGet.Packaging.PackagingConstants;

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

        group.MapGet("/{id}", async Task<Results<Ok<Orders>, NotFound>> (Guid orderid, MainDatabaseContext db) =>
        {
            return await db.Orders.AsNoTracking()
                .FirstOrDefaultAsync(model => model.OrderId == orderid)
                is Orders model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetOrdersById")
        .WithOpenApi();

        // get foodIDs of a given order
        group.MapGet("/order_food", async (Guid orderId, MainDatabaseContext db) =>
        {
            var connectionString = db.Database.GetConnectionString();
            var sql = $"SELECT food_id FROM Order_Foods WHERE order_id = '{orderId}'";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(sql, connection))
                {
                    var foodIds = new List<Guid>();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            if (reader[0] != DBNull.Value)
                            {
                                foodIds.Add(reader.GetGuid(0));
                            }
                        }
                    }

                    return TypedResults.Ok(foodIds);
                }
            }
        })
        .WithName("GetFoodIdsByOrderId")
        .WithOpenApi();

        // updating order status
        group.MapPut("/putStatus", async Task<Results<Ok, NotFound , BadRequest<string>>> (Guid? orderid, string? status, Orders? orders, MainDatabaseContext db) =>
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

        group.MapPost("/", async Task<Results<Created<Orders>, NotFound, BadRequest<string>>> (Orders orders, MainDatabaseContext db) =>
        {
            // Retrieve Reservation Information
            var reservationInfo = await db.Reservation
                .Where(r => r.ReservationId == orders.ReservationId)
                .Select(r => new
                {
                    r.ReservationId,
                    r.StaffId
                })
                .FirstOrDefaultAsync();

            if (reservationInfo == null)
            {
                return TypedResults.NotFound(); // Return a 404 Not Found response
            }

            // Check Staff Availability (for now)
            // better to check with an arrived column
            if (reservationInfo.StaffId == null)
            {
                return TypedResults.BadRequest("Staff is not assigned to this reservation. Cannot create order."); // Return a 400 Bad Request response
            }

            orders.OrderId = Guid.NewGuid();
            if (orders.ReservationId == null)
            {
                orders.ReservationId = Guid.Empty;
            }
            if (orders.Total == 0)
            {
                orders.Total = 0;
            }
            if (orders.OrderStatus != "pending")
            {
                orders.OrderStatus = "pending";
            }

            if (orders.Discount != 0)
            {
                orders.Price = orders.Total - orders.Discount;
            }
            else
            {
                orders.Price = orders.Total;
            }

            db.Orders.Add(orders);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Orders/{orders.OrderId}",orders);
        })
        .WithName("CreateOrders")
        .WithOpenApi();

        // adding food to an order
        group.MapPost("/order_food", async Task<Results<Ok<string>, NotFound<string>>> (Guid orderId, Guid foodId, MainDatabaseContext db) =>
        {
            // Check if the order and food exist
            var order = await db.Orders.FindAsync(orderId);
            var food = await db.Food.FindAsync(foodId);

            if (order == null || food == null)
            {
                return TypedResults.NotFound("Order or Food not found.");
            }

            var sql = $"INSERT INTO Order_Foods (order_id, food_id) VALUES ('{orderId}', '{foodId}')";
            await db.Database.ExecuteSqlRawAsync(sql);

            //get food price 
            var food_price = await db.Food
                             .Where(f => f.FoodId == foodId)
                             .Select(f => f.Price)
                             .FirstOrDefaultAsync();
            // get food total
            var orderTot = await db.Orders
                            .Where(o => o.OrderId == orderId)
                            .Select(o => o.Total)
                            .FirstOrDefaultAsync();
            orderTot = orderTot + food_price;

            // update total in orders table
            var affected = await db.Orders
                .Where(model => model.OrderId == orderId)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Total, orderTot)
                );

            //get customer id of the orders
            var customerinfo = await (from o in db.Orders
                                join r in db.Reservation on o.ReservationId equals r.ReservationId
                                where o.OrderId == orderId
                                select r.CustomerId)
                                .FirstOrDefaultAsync();

            // Check if a row exists in FoodUser for customerinfo and foodId
            var existingFoodUser = await db.FoodUser
                            .Where(fu => fu.CustomerId == customerinfo && fu.FoodId == foodId)
                            .FirstOrDefaultAsync();

            if (existingFoodUser != null)
            {
                // Row exists, increment quantity
                existingFoodUser.OrderCount += 1;
            }
            else
            {
                // Row doesn't exist, insert a new row and Set initial quantity to 1
                var newFoodUser = new FoodUser
                {
                    CustomerId = customerinfo,
                    FoodId = foodId,
                    OrderCount = 1 
                };
                db.FoodUser.Add(newFoodUser);
            }

            // Save changes to FoodUser table
            await db.SaveChangesAsync();

            return TypedResults.Ok("Order-Food relationship created successfully.");
        })
        .WithName("CreateOrderFoodRelationship")
        .WithOpenApi();

        //delete an order
        group.MapDelete("/{id}", async Task<Results<Ok, BadRequest, NotFound>> (Guid orderid, MainDatabaseContext db) =>
        {
            var order = await db.Orders.FindAsync(orderid);
            if (order == null)
            {
                return TypedResults.NotFound();
            }
            // Check if the order status is "pending"
            if (order.OrderStatus != "pending")
            {
                return TypedResults.BadRequest();
            }

            // to 1st delete from OrderFood table 
            // SQL DELETE statement
            var sql = $"DELETE FROM Order_Foods WHERE order_id = '{orderid}'";

            // Execute the DELETE statement on OrderFood table
            await db.Database.ExecuteSqlRawAsync(sql);

            var affected = await db.Orders
                .Where(model => model.OrderId == orderid)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteOrders")
        .WithOpenApi();

        // delete food from an order
        group.MapDelete("/{orderId}/{foodId}", async Task<Results<Ok<string>,BadRequest, NotFound>> (Guid orderId, Guid foodId, MainDatabaseContext db) =>
        {
            // Check if the order and food exist
            var order = await db.Orders.FindAsync(orderId);
            var food = await db.Food.FindAsync(foodId);

            if (order == null || food == null)
            {
                return TypedResults.NotFound();
            }

            // Check if the order status is "pending"
            if (order.OrderStatus != "pending")
            {
                return TypedResults.BadRequest();
            }

            // SQL DELETE statement
            var sql = $"DELETE FROM Order_Foods WHERE order_id = '{orderId}' AND food_id = '{foodId}'";

            // Execute the DELETE statement
            await db.Database.ExecuteSqlRawAsync(sql);

            return TypedResults.Ok("Order-Food relationship deleted successfully.");
        })
        .WithName("DeleteOrderFoodRelationship")
        .WithOpenApi();

    }

}
