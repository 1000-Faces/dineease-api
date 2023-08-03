using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using webapi.Models;
namespace webapi.Endpoints;

public static class ReservationEndpoints
{
    public static void MapReservationEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Reservation").WithTags(nameof(Reservation));

        group.MapGet("/", async (MainDatabaseContext db) =>
        {
            return await db.Reservation.ToListAsync();
        })
        .WithName("GetAllReservations")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Reservation>, NotFound>> (string reservationid, MainDatabaseContext db) =>
        {
            return await db.Reservation.AsNoTracking()
                .FirstOrDefaultAsync(model => model.ReservationId == reservationid)
                is Reservation model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetReservationById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (string reservationid, Reservation reservation, MainDatabaseContext db) =>
        {
            var affected = await db.Reservation
                .Where(model => model.ReservationId == reservationid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.ReservationId, reservation.ReservationId)
                  .SetProperty(m => m.CustomerId, reservation.CustomerId)
                  .SetProperty(m => m.StaffId, reservation.StaffId)
                  .SetProperty(m => m.TableNo, reservation.TableNo)
                  .SetProperty(m => m.ReservationDatetime, reservation.ReservationDatetime)
                  .SetProperty(m => m.Departure, reservation.Departure)
                  .SetProperty(m => m.ActualDeparture, reservation.ActualDeparture)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateReservation")
        .WithOpenApi();

        group.MapPost("/", async (Reservation reservation, MainDatabaseContext db) =>
        {
            db.Reservation.Add(reservation);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Reservation/{reservation.ReservationId}",reservation);
        })
        .WithName("CreateReservation")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (string reservationid, MainDatabaseContext db) =>
        {
            var affected = await db.Reservation
                .Where(model => model.ReservationId == reservationid)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteReservation")
        .WithOpenApi();
    }
}
