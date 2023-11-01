using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OpenApi;
using webapi.Models;
using webapi.Services;
using NuGet.Packaging.Signing;
using System.Text.Json.Serialization;
using System.Text.Json;
using static NuGet.Packaging.PackagingConstants;
using Stripe.Checkout;
using Stripe;
using Azure;

namespace webapi.Endpoints;

public static class ReservationEndpoints
{
    public static void MapReservationEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Reservation").WithTags(nameof(Reservation));

        group.MapGet("/", async (MainDatabaseContext db) =>
        {
            return await db.Reservation
            .Where(r => r.Status != "opened")
            .ToListAsync();
        })
        .WithName("GetAllReservations")
        .WithOpenApi();

        group.MapGet("/today", async (MainDatabaseContext db) =>
        {
            DateTime today = DateTime.Now.Date; // Get today's date without the time

            var reservations = await db.Reservation
                .Where(r => (r.Departure.HasValue && r.Departure.Value > DateTime.Now && r.Departure.Value.Date == today) && 
                            r.Status != "opened")
                .ToListAsync();

            return reservations;
        })
        .WithName("GetReservationsForToday")
        .WithOpenApi();

        // provides the reservations overlapping a given time
        // used to fine all occupied tables
        group.MapGet("/SelectedTime", async Task<Results<Ok<List<int?>>, NotFound>> (DateTime reservationDatetime, MainDatabaseContext db) =>
        {
            DateTime today = DateTime.Now.Date; // Get today's date without the time
            var departure = reservationDatetime.AddHours(2);
            var tableNumbers = await db.Reservation
                        .Where(r => r.ReservationDatetime.HasValue && r.ReservationDatetime.Value.Date == reservationDatetime.Date &&
                        ((r.ReservationDatetime <= reservationDatetime && r.Departure >= reservationDatetime) ||
                        (r.ReservationDatetime <= departure && r.Departure >= departure)))
                        .Select(r => r.TableNo) // Select the TableNo property from reservations
                        .ToListAsync();

            if (tableNumbers.Any()) // Check if there are any table numbers found
            {
                return TypedResults.Ok(tableNumbers);
            }

            return TypedResults.NotFound();


        })
        .WithName("GetReservationsSelectedTime")
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

        //makeing a reservation
        //group.MapPost("/", async Task<Results<Created<Reservation>, BadRequest<string>>> (Reservation reservation, MainDatabaseContext db) =>
        group.MapPost("/", async (Reservation reservation, MainDatabaseContext db) =>
        {
            // Task<IResult, Results<Ok, Created<Reservation> , NotFound<string>, BadRequest<string>, BadRequest<Exception>>> 
            try
            {
                // Check for reservation overlap
                var overlappingReservation = await db.Reservation
                    .Where(r => r.ReservationDatetime.HasValue && reservation.ReservationDatetime.HasValue &&
                                r.ReservationDatetime.Value.Date == reservation.ReservationDatetime.Value.Date &&
                                r.TableNo == reservation.TableNo &&
                                ((r.ReservationDatetime <= reservation.ReservationDatetime && r.Departure >= reservation.ReservationDatetime) ||
                                (r.ReservationDatetime <= reservation.Departure && r.Departure >= reservation.Departure)))
                    .Select(r => r.ReservationId)
                    .FirstOrDefaultAsync();

                if (overlappingReservation != Guid.Empty)
                {
                    // There is an overlap with existing reservation(s)
                    return TypedResults.BadRequest("This table is being processed. Please chose another Table");
                }

                reservation.ReservationId = Guid.NewGuid();
                var PtsToUpdate = await db.Customer
                    .Where(model => model.UserId == reservation.CustomerId)
                    .Select(model => model.LoyalityPts)
                    .FirstOrDefaultAsync();

                // Find the customer and update Loyality_pts
                var affected = await db.Customer
                    .Where(model => model.UserId == reservation.CustomerId)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(m => m.LoyalityPts, PtsToUpdate + 10)
                    );

                // for dev perposses, res time is set to current date time 
                // it will be provided on the post req
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
                //return TypedResults.Created($"/api/Reservation/{reservation.ReservationId}", reservation);

                // Stripe payments start

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
                        UnitAmount = 1000,
                        Currency = "inr",
                        ProductData = new SessionLineItemPriceDataProductDataOptions()
                        {
                            Name = "reservation"
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

                if (redirectUrl == null)
                {
                    return Results.Redirect(redirectUrl);
                }
                
                // Set the redirect URL in the response headers
                // Response.Headers.Add("Location", session.Url);
                // to do : redirect to the session.Url from within the endpoint

                
                // return TypedResults.Created($"/api/Reservation/{reservation.ReservationId}", reservation);

            }
            catch (Exception ex)
            {
                // Handle other exceptions (e.g., database errors)
                return TypedResults.BadRequest(ex);
            }
            // Stripe payments end

            //db.Reservation.Add(reservation);
            //await db.SaveChangesAsync();
            //return TypedResults.Created($"/api/Reservation/{reservation.ReservationId}",reservation);
        })
        .WithName("CreateReservation")
        .WithOpenApi();

        // deleting a reervation by reservationID
        group.MapDelete("/{id}", async Task<Results<Ok, NotFound, BadRequest>> (Guid reservationid, MainDatabaseContext db) =>
        {
            // Retrieve Reservation Information
            var reservationInfo = await db.Reservation
                .Where(r => r.ReservationId == reservationid)
                .Select(r => new
                {
                    r.CustomerId,
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
                return TypedResults.BadRequest(); // Return a 400 Bad Request response
            }

            var PtsToUpdate = await db.Customer
                .Where(model => model.UserId == reservationInfo.CustomerId)
                .Select(model => model.LoyalityPts)
                .FirstOrDefaultAsync();

            // Find the customer and update Loyality_pts
            var affectedCustomer = await db.Customer
                .Where(model => model.UserId == reservationInfo.CustomerId)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.LoyalityPts, PtsToUpdate - 10)
                );

            var affected = await db.Reservation
                .Where(model => model.ReservationId == reservationid)
                .ExecuteDeleteAsync();

            await db.SaveChangesAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteReservation")
        .WithOpenApi();

        // assigning staff
        group.MapPut("/update_staff", async Task<Results<Ok, NotFound>> (Guid reservationid, MainDatabaseContext db) =>
        {
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

            // Update is_active column in the Staff table for the selected user_id
            var staffToUpdate = await db.Staff
                .Where(staff => staff.UserId == randomUserId)
                .FirstOrDefaultAsync();

            if (staffToUpdate == null)
            {
                return TypedResults.NotFound(); // Staff record not found
            }

            staffToUpdate.IsActive = 0;
            db.Staff.Update(staffToUpdate);

            // Update staff_id column in the Reservation table for the given reservationid
            var reservationToUpdate = await db.Reservation
                .Where(reservation => reservation.ReservationId == reservationid)
                .FirstOrDefaultAsync();

            if (reservationToUpdate == null)
            {
                return TypedResults.NotFound(); // Reservation record not found
            }

            reservationToUpdate.StaffId = randomUserId;
            reservationToUpdate.Status = "arrived";
            db.Reservation.Update(reservationToUpdate);

            await db.SaveChangesAsync();

            return TypedResults.Ok();
        })
        .WithName("Update staff for reservation")
        .WithOpenApi();

    }
}
