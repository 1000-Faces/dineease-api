using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using webapi.Models;
namespace webapi.Endpoints;

public static class StaffEndpoints
{
    public static void MapStaffEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Staff").WithTags(nameof(Staff));

        group.MapGet("/", async (MainDatabaseContext db) =>
        {
            return await db.Staff.ToListAsync();
        })
        .WithName("GetAllStaff")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Staff>, NotFound>> (Guid userid, MainDatabaseContext db) =>
        {
            return await db.Staff.AsNoTracking()
                .FirstOrDefaultAsync(model => model.UserId == userid)
                is Staff model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetStaffById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (Guid userid, Staff staff, MainDatabaseContext db) =>
        {
            var affected = await db.Staff
                .Where(model => model.UserId == userid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.UserId, staff.UserId)
                  .SetProperty(m => m.JobTitle, staff.JobTitle)
                  .SetProperty(m => m.IsActive, staff.IsActive)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateStaff")
        .WithOpenApi();

        // Update unavailability of staff
        group.MapPut("/unavailable", async Task<Results<Ok, NotFound>> (Guid userid, MainDatabaseContext db) =>
        {
            // setting staff user to unavailable
            var affected = await db.Staff
                .Where(model => model.UserId == userid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.IsActive, (byte?)0)       // casting int to byte
                );


            // Checking for reservations assigned to unavailable staff
            var reservationToUpdate = await db.Reservation
                .Where(reservation => reservation.StaffId == userid && reservation.ReservationDatetime < DateTime.Now 
                && DateTime.Now < reservation.Departure)
                .FirstOrDefaultAsync();

            if (!(reservationToUpdate == null))
            {
                // assignig new staff to current reservation
                // Get a random active user_id from the Staff table
                var randomUserId = await db.Staff
                    .Where(staff => staff.IsActive == 1)
                    .OrderBy(_ => Guid.NewGuid())
                    .Select(staff => staff.UserId)
                    .FirstOrDefaultAsync();

                if (randomUserId == default(Guid))
                {
                    return TypedResults.NotFound(); // No active staff found
                }

                var update2 = await db.Staff
                    .Where(model => model.UserId == randomUserId)
                    .ExecuteUpdateAsync(setters => setters
                      .SetProperty(m => m.IsActive, (byte?)0)       // casting int to byte
                    );

                reservationToUpdate.StaffId = randomUserId;
                db.Reservation.Update(reservationToUpdate);

                await db.SaveChangesAsync();
            }

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateToUnavailable")
        .WithOpenApi();

        // Update unavailability of staff
        group.MapPut("/available", async Task<Results<Ok, NotFound>> (Guid userid, MainDatabaseContext db) =>
        {
            var affected = await db.Staff
                .Where(model => model.UserId == userid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.IsActive, (byte?)1)       // casting int to byte
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateToAvailable")
        .WithOpenApi();

        group.MapPost("/", async (Staff staff, MainDatabaseContext db) =>
        {
            db.Staff.Add(staff);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Staff/{staff.UserId}",staff);
        })
        .WithName("CreateStaff")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (Guid userid, MainDatabaseContext db) =>
        {
            var affected = await db.Staff
                .Where(model => model.UserId == userid)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteStaff")
        .WithOpenApi();
    }
}
