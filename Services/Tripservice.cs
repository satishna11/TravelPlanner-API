using Microsoft.EntityFrameworkCore;
using TravelAI.Data;
using TravelAI.Models.DTOs;
using TravelAI.Models.Entities;

namespace TravelAI.Services
{
    public class TripService
    {
        private readonly AppDbContext _context;

        public TripService(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveTrip(int userId, TripSaveRequest request)
        {
            var trip = new Trip
            {
                UserId = userId,
                TravelDate = request.TripRequest.TravelDate,
                Days = request.TripRequest.Days,
                Budget = request.TripRequest.Budget,
                Travellers = request.TripRequest.Travellers,
                Transportation = request.TripRequest.Transportation,
                HotelCategory = request.TripRequest.HotelCategory
            };

            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();

            _context.TripDestinations.Add(new TripDestination
            {
                TripId = trip.TripId,
                DestinationId = request.DestinationId,
                Sequence = 1
            });

            foreach (var day in request.Itinerary.Days)
            {
                var itinerary = new Itinerary
                {
                    TripId = trip.TripId,
                    DayNumber = day.Day,
                    Morning = day.Activities.Count > 0 ? day.Activities[0] : "",
                    Afternoon = day.Activities.Count > 1 ? day.Activities[1] : "",
                    Evening = day.Activities.Count > 2 ? day.Activities[2] : "",
                    EstimatedCost = 3000
                };

                _context.Itineraries.Add(itinerary);
            }

            await _context.SaveChangesAsync();
        }
    }
}