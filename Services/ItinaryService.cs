using Microsoft.EntityFrameworkCore;
using TravelAI.Data;
using TravelAI.Models.DTOs;
using TravelAI.Models.Entities;

public class ItineraryService
{
    private readonly AppDbContext _context;

    public ItineraryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ItenaryResponse.ItineraryResponse> Generate(
        int userId,
        TripRequest request,
        int destinationId)
    {
        // Get destination
        var destination = await _context.Destinations
            .FirstAsync(x => x.DestinationId == destinationId);

        // Select hotel
        var hotel = await _context.Hotels
            .Where(x => x.DestinationId == destinationId &&
                        x.Category == request.HotelCategory)
            .OrderByDescending(x => x.Rating)
            .FirstOrDefaultAsync();

        if (hotel == null)
        {
            hotel = await _context.Hotels
                .Where(x => x.DestinationId == destinationId)
                .OrderByDescending(x => x.Rating)
                .FirstOrDefaultAsync();
        }

        // Load activities
        var activities = await _context.DestinationActivity
            .Where(x => x.DestinationId == destinationId)
            .ToListAsync();

        // ============================
        // SAVE TRIP
        // ============================

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

        _context.TripDestinations.Add(new TripDestination
        {
            TripId = trip.TripId,
            DestinationId = destinationId,
            Sequence = 1
        });

        await _context.SaveChangesAsync();

        // Response
        var response = new ItenaryResponse.ItineraryResponse
        {
            Destination = destination.Name,
            HotelName = hotel?.HotelName ?? "",
            Transportation = request.Transportation,
            EstimatedBudget = request.Budget
        };

        int index = 0;

        for (int day = 1; day <= request.Days; day++)
        {
            var plan = new DayPlan
            {
                Day = day
            };

            string morningActivity = "";
            string afternoonActivity = "";
            string eveningActivity = "";

            if (day == 1)
            {
                morningActivity = $"Travel to {destination.Name}";
                afternoonActivity = $"Check in at {hotel?.HotelName}";

                plan.Activities.Add(morningActivity);
                plan.Activities.Add(afternoonActivity);

                var evening = activities.FirstOrDefault(x => x.TimeSlot == "Evening");

                if (evening != null)
                {
                    eveningActivity = evening.ActivityName;
                    plan.Activities.Add(eveningActivity);
                }

                plan.Activities.Add("Dinner at local restaurant");
            }
            else if (day == request.Days)
            {
                plan.Activities.Add("Breakfast");

                var morning = activities.Skip(index)
                    .FirstOrDefault(x => x.TimeSlot == "Morning");

                if (morning != null)
                {
                    morningActivity = morning.ActivityName;
                    plan.Activities.Add(morningActivity);
                    index++;
                }

                afternoonActivity = "Shopping";
                eveningActivity = "Return Journey";

                plan.Activities.Add(afternoonActivity);
                plan.Activities.Add("Hotel Check-out");
                plan.Activities.Add(eveningActivity);
            }
            else
            {
                plan.Activities.Add("Breakfast");

                var morning = activities.Skip(index)
                    .FirstOrDefault(x => x.TimeSlot == "Morning");

                if (morning != null)
                {
                    morningActivity = morning.ActivityName;
                    plan.Activities.Add(morningActivity);
                    index++;
                }

                var afternoon = activities.Skip(index)
                    .FirstOrDefault(x => x.TimeSlot == "Afternoon");

                if (afternoon != null)
                {
                    afternoonActivity = afternoon.ActivityName;
                    plan.Activities.Add(afternoonActivity);
                    index++;
                }

                var evening = activities.Skip(index)
                    .FirstOrDefault(x => x.TimeSlot == "Evening");

                if (evening != null)
                {
                    eveningActivity = evening.ActivityName;
                    plan.Activities.Add(eveningActivity);
                    index++;
                }

                plan.Activities.Add("Dinner");
            }

            // ============================
            // SAVE ITINERARY
            // ============================

            var itinerary = new Itinerary
            {
                TripId = trip.TripId,
                DestinationId = destinationId,
                DayNumber = day,
                Morning = morningActivity,
                Afternoon = afternoonActivity,
                Evening = eveningActivity,
                EstimatedCost = 3000 // You can calculate this later
            };

            _context.Itineraries.Add(itinerary);

            response.Days.Add(plan);
        }

        await _context.SaveChangesAsync();

        return response;
    }
}