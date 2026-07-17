using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using TravelAI.Data;
using TravelAI.Models.DTOs;
using TravelAI.Models.Entities;

namespace TravelAI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User")]
    public class TripController : BaseController
    {
        private readonly AppDbContext _context;

        public TripController(AppDbContext context)
        {
            _context = context;
        }


        [HttpPost("save")]
        public async Task<IActionResult> SaveTrip(
            [FromBody] SaveTripRequest request)
        {

            if (request == null)
            {
                return BadRequest("Request is empty.");
            }


            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );


            if(request.DestinationId == null)
            {
                return BadRequest("Destination required.");
            }

            var trip = new Trip
            {
                UserId = userId,
                TravelDate = request.TravelDate,
                Days = request.Days,
                Budget = request.Budget,
                Travellers = request.Travellers,
                Transportation = request.Transportation,
                HotelCategory = request.HotelCategory
            };


            _context.Trips.Add(trip);

            await _context.SaveChangesAsync();



            // ============================
            // SAVE TRIP DESTINATION
            // ============================

            var tripDestination = new TripDestination
            {
                TripId = trip.TripId,
                DestinationId = request.DestinationId
            };


            _context.TripDestinations.Add(tripDestination);

            await _context.SaveChangesAsync();



            // ============================
            // SAVE ITINERARY
            // ============================
            if (request.Itineraries != null && request.Itineraries.Any())
            {
                foreach (var day in request.Itineraries)
                {
                    var itinerary = new Itinerary
                    {
                        TripId = trip.TripId,
                        DestinationId = request.DestinationId,
                        DayNumber = day.DayNumber,
                        Morning = day.Morning,
                        Afternoon = day.Afternoon,
                        Evening = day.Evening,
                        EstimatedCost = day.EstimatedCost
                    };

                    _context.Itineraries.Add(itinerary);
                }

                await _context.SaveChangesAsync();
            }


            return Success(new
            {
                TripId = trip.TripId
            },
            "Trip saved successfully.");

        }
    [HttpGet("my-trips")]
    public async Task<IActionResult> GetMyTrips()
    {
        var userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value
        );

        var trips = await _context.Trips
            .Where(t => t.UserId == userId)
            .Select(t => new
            {
                t.TripId,
                t.TravelDate,
                t.Budget,
                t.Days,
                t.HotelCategory,
                t.Transportation,
                t.Status,
                Destination = _context.TripDestinations
                    .Where(td => td.TripId == t.TripId)
                    .Join(
                        _context.Destinations,
                        td => td.DestinationId,
                        d => d.DestinationId,
                        (td, d) => new
                        {
                            d.Name,
                            d.ImageUrl
                        })
                    .FirstOrDefault()
            })
            .OrderByDescending(t => t.TravelDate)
            .ToListAsync();

        return Success(trips, "My trips fetched successfully.");
    }
    [HttpGet("{tripId}")]
    public async Task<IActionResult> GetTripDetails(int tripId)
    {
        var userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value
        );

        var trip = await _context.Trips
            .Where(t => t.TripId == tripId && t.UserId == userId)
            .Select(t => new
            {
                t.TripId,
                t.TravelDate,
                t.Days,
                t.Budget,
                t.Travellers,
                t.Transportation,
                t.HotelCategory,

                Destination = _context.TripDestinations
                    .Where(td => td.TripId == t.TripId)
                    .Join(
                        _context.Destinations,
                        td => td.DestinationId,
                        d => d.DestinationId,
                        (td, d) => new
                        {
                            d.Name,
                            d.City,
                            d.Country,
                            d.Description,
                            d.ImageUrl
                        })
                    .FirstOrDefault(),

                Itineraries = _context.Itineraries
                    .Where(i => i.TripId == t.TripId)
                    .OrderBy(i => i.DayNumber)
                    .Select(i => new
                    {
                        i.DayNumber,
                        i.Morning,
                        i.Afternoon,
                        i.Evening,
                        i.EstimatedCost
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        if (trip == null)
        {
            return Fail("Trip not found.");
        }

        return Success(trip, "Trip details fetched successfully.");
    }
    [HttpPut("{tripId}/complete")]
    public async Task<IActionResult> CompleteTrip(int tripId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var trip = await _context.Trips
            .FirstOrDefaultAsync(x =>
                x.TripId == tripId &&
                x.UserId == userId);

        if (trip == null)
            return Fail("Trip not found.");

        trip.Status = "Completed";

        await _context.SaveChangesAsync();

        return Success(null, "Trip marked as completed.");
    }
    [HttpDelete("{tripId}")]
    public async Task<IActionResult> DeleteTrip(int tripId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var trip = await _context.Trips
            .FirstOrDefaultAsync(x =>
                x.TripId == tripId &&
                x.UserId == userId);

        if (trip == null)
            return Fail("Trip not found.");

        var itineraries = _context.Itineraries
            .Where(i => i.TripId == tripId);

        _context.Itineraries.RemoveRange(itineraries);

        var tripDestinations = _context.TripDestinations
            .Where(td => td.TripId == tripId);

        _context.TripDestinations.RemoveRange(tripDestinations);

        _context.Trips.Remove(trip);

        await _context.SaveChangesAsync();

        return Success(null, "Trip deleted successfully.");
    }
    }
   
}