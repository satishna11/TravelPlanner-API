using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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



            return Success(new
            {
                TripId = trip.TripId
            },
            "Trip saved successfully.");

        }
        
    }
}