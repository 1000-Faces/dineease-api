using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using webapi.Models;
using NuGet.Packaging.Signing;

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

        group.MapGet("/today", async (MainDatabaseContext db) =>
        {
            DateTime today = DateTime.Now.Date; // Get today's date without the time

            var reservations = await db.Reservation
                .Where(r => r.Departure.HasValue && r.Departure.Value > DateTime.Now && r.Departure.Value.Date == today)
                .ToListAsync();

            return reservations;
        })
        .WithName("GetReservationsForToday")
        .WithOpenApi();

        //group.MapGet("/{id}", async Task<Results<Ok<Reservation>, NotFound>> (string reservationid, MainDatabaseContext db) =>
        group.MapGet("/{id}", async Task<Results<Ok<Reservation>, NotFound>> (Guid reservationid, MainDatabaseContext db) =>
        {
            return await db.Reservation.AsNoTracking()
                .FirstOrDefaultAsync(model => model.ReservationId == reservationid)
                is Reservation model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetReservationById")
        .WithOpenApi();

        //group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (Guid reservationid, Reservation reservation, MainDatabaseContext db) =>
        //{
        //    var affected = await db.Reservation
        //        .Where(model => model.ReservationId == reservationid)
        //        .ExecuteUpdateAsync(setters => setters
        //          //.SetProperty(m => m.ReservationId, reservation.ReservationId)
        //          //.SetProperty(m => m.CustomerId, reservation.CustomerId)
        //          //.SetProperty(m => m.StaffId, reservation.StaffId)
        //          .SetProperty(m => m.TableNo, reservation.TableNo)
        //          .SetProperty(m => m.ReservationDatetime, reservation.ReservationDatetime)
        //          .SetProperty(m => m.Departure, reservation.Departure)
        //          // connot update a time stamp column
        //          //.SetProperty(m => m.ActualDeparture, reservation.ActualDeparture)
        //        );

        //    return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        //})
        //.WithName("UpdateReservation")
        //.WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (Guid reservationid, Reservation reservation, MainDatabaseContext db) =>
        {
            var reservationToUpdate = await db.Reservation
                .Where(model => model.ReservationId == reservationid)
                .FirstOrDefaultAsync();

            if (reservationToUpdate != null)
            {
                // Update TableNo, ReservationDatetime, and Departure if values are provided
                if (reservation.TableNo != 0)
                {
                    reservationToUpdate.TableNo = reservation.TableNo;
                }
                if (reservation.ReservationDatetime != null)
                {
                    reservationToUpdate.ReservationDatetime = reservation.ReservationDatetime;
                    reservationToUpdate.Departure = reservation.ReservationDatetime.Value.AddHours(2);
                }

                // Save changes to the database
                await db.SaveChangesAsync();

                return TypedResults.Ok();
            }
            else
            {
                return TypedResults.NotFound();
            }
        })
        .WithName("UpdateReservation")
        .WithOpenApi();

        // updating departue time
        group.MapPut("/departure", async Task<Results<Ok, NotFound>> (Guid reservationid, MainDatabaseContext db) =>
        {
            var affected = await db.Reservation
                .Where(model => model.ReservationId == reservationid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Departure, DateTime.Now)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("Update departure time")
        .WithOpenApi();


        //group.MapPut("/departure/{id}", async Task<Results<Ok, NotFound>> (Guid reservationid, MainDatabaseContext db) =>
        //{
        //    var reservation = await db.Reservation
        //        .FirstOrDefaultAsync(model => model.ReservationId == reservationid);

        //    if (reservation != null)
        //    {
        //        reservation.ActualDeparture = BitConverter.GetBytes(DateTime.Now.Ticks);
        //        db.Update(reservation);
        //        await db.SaveChangesAsync();
        //        return TypedResults.Ok();
        //    }
        //    else
        //    {
        //        return TypedResults.NotFound();
        //    }
        //})
        //.WithName("UpdateActualDeparture")
        //.WithOpenApi();




        group.MapPost("/", async (Reservation reservation, MainDatabaseContext db) =>
        {
            reservation.ReservationId = Guid.NewGuid();
            // for dev perposses, res time is set to current date time 
            // it shold be provided on the post req
            if (reservation.ReservationDatetime == null)
            {
                reservation.ReservationDatetime = DateTime.Now.AddHours(1);
                reservation.Departure = reservation.ReservationDatetime.Value.AddHours(2);
            }
            else
            {
                reservation.Departure = reservation.ReservationDatetime.Value.AddHours(2);
            }
            db.Reservation.Add(reservation);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Reservation/{reservation.ReservationId}",reservation);
        })
        .WithName("CreateReservation")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (Guid reservationid, MainDatabaseContext db) =>
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
