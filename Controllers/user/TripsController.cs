using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAI.Data;
using TravelAI.Models.DTOs;
using TravelAI.Models.Entities;
using TravelAI.Services;

namespace TravelAI.Controllers.User
{
    [ApiController]
    [Route("api/user/trip")]
    [Authorize(Roles = "User")]
    public class TripController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly TripService _tripService;

        public TripController(AppDbContext context, TripService tripService)
        {
            _context = context;
            _tripService = tripService;
        }

        // ==========================
        // SAVE TRIP
        // ==========================
        [HttpPost("save")]
        public async Task<IActionResult> SaveTrip([FromBody] TripSaveRequest request)
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await _tripService.SaveTrip(userId, request);

            return Success(null, "Trip saved successfully.");
        }

        // ==========================
        // GET MY TRIPS
        // ==========================
        [HttpGet]
        public async Task<IActionResult> GetMyTrips()
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var trips = await _context.Trips
                .Where(t => t.UserId == userId)
                .Include(t => t.TripDestinations)
                    .ThenInclude(td => td.Destination)
                .OrderByDescending(t => t.TravelDate)
                .Select(t => new
                {
                    t.TripId,
                    t.TravelDate,
                    t.Days,
                    t.Budget,
                    t.Travellers,
                    t.Transportation,
                    t.HotelCategory,

                    Destinations = t.TripDestinations.Select(td => new
                    {
                        td.DestinationId,
                        td.Sequence,
                        td.Destination.Name,
                        td.Destination.City,
                        td.Destination.ImageUrl
                    })
                })
                .ToListAsync();

            return Success(trips);
        }

        // ==========================
        // GET TRIP DETAIL
        // ==========================
        [HttpGet("{tripId}")]
        public async Task<IActionResult> GetTripDetail(int tripId)
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var trip = await _context.Trips
                .Include(t => t.TripDestinations)
                    .ThenInclude(td => td.Destination)
                .Include(t => t.Itineraries)
                .FirstOrDefaultAsync(t =>
                    t.TripId == tripId &&
                    t.UserId == userId);

            if (trip == null)
                return NotFoundResponse("Trip not found.");

            var response = new
            {
                trip.TripId,
                trip.TravelDate,
                trip.Days,
                trip.Budget,
                trip.Travellers,
                trip.Transportation,
                trip.HotelCategory,

                Destinations = trip.TripDestinations
                    .OrderBy(x => x.Sequence)
                    .Select(x => new
                    {
                        x.Sequence,
                        x.DestinationId,
                        x.Destination.Name,
                        x.Destination.City,
                        x.Destination.Country,
                        x.Destination.Description,
                        x.Destination.ImageUrl
                    }),

                Itinerary = trip.Itineraries
                    .OrderBy(x => x.DayNumber)
                    .Select(x => new
                    {
                        x.DayNumber,
                        x.Morning,
                        x.Afternoon,
                        x.Evening,
                        x.EstimatedCost
                    })
            };

            return Success(response);
        }

        // ==========================
        // DELETE TRIP
        // ==========================
        [HttpDelete("{tripId}")]
        public async Task<IActionResult> DeleteTrip(int tripId)
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var trip = await _context.Trips
                .Include(t => t.Itineraries)
                .Include(t => t.TripDestinations)
                .FirstOrDefaultAsync(t =>
                    t.TripId == tripId &&
                    t.UserId == userId);

            if (trip == null)
                return NotFoundResponse("Trip not found.");

            _context.Itineraries.RemoveRange(trip.Itineraries);
            _context.TripDestinations.RemoveRange(trip.TripDestinations);
            _context.Trips.Remove(trip);

            await _context.SaveChangesAsync();

            return Success(null, "Trip deleted successfully.");
        }
    }
}