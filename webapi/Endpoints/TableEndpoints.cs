using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using webapi.Models;
namespace webapi.Endpoints;

public static class TableEndpoints
{
    public static void MapTableEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Table").WithTags(nameof(Table));

        group.MapGet("/", async (MainDatabaseContext db) =>
        {
            return await db.Table.ToListAsync();
        })
        .WithName("GetAllTables")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Table>, NotFound>> (int tableno, MainDatabaseContext db) =>
        {
            return await db.Table.AsNoTracking()
                .FirstOrDefaultAsync(model => model.TableNo == tableno)
                is Table model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetTableById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int tableno, Table table, MainDatabaseContext db) =>
        {
            var affected = await db.Table
                .Where(model => model.TableNo == tableno)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.TableNo, table.TableNo)
                  .SetProperty(m => m.Seating, table.Seating)
                  .SetProperty(m => m.Availability, table.Availability)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateTable")
        .WithOpenApi();

        group.MapPost("/", async (Table table, MainDatabaseContext db) =>
        {
            int newTableNo;

            // Check if there are any existing rows in the table
            if (await db.Table.AnyAsync())
            {
                // Get the maximum value of tableNo from existing rows
                int maxTableNo = await db.Table.MaxAsync(t => t.TableNo);

                // Calculate the new tableNo as n + 1
                newTableNo = maxTableNo + 1;
            }
            else
            {
                // No rows in the table, set newTableNo to 1
                newTableNo = 1;
            }

            // Assign the calculated tableNo to the new entry
            table.TableNo = newTableNo;

            db.Table.Add(table);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Table/{table.TableNo}",table);
        })
        .WithName("CreateTable")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int tableno, MainDatabaseContext db) =>
        {
            var affected = await db.Table
                .Where(model => model.TableNo == tableno)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteTable")
        .WithOpenApi();
    }
}
