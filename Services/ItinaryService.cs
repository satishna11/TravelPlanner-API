using Microsoft.EntityFrameworkCore;
using TravelAI.Data;
using TravelAI.Models.DTOs;

public class ItineraryService
{
    private readonly AppDbContext _context;

    public ItineraryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ItenaryResponse.ItineraryResponse> Generate(TripRequest request, int destinationId)
    {
        var destination = await _context.Destinations
            .FirstAsync(x => x.DestinationId == destinationId);

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

        var activities = await _context.DestinationActivity
            .Where(x => x.DestinationId == destinationId)
            .ToListAsync();

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
            var plan = new DayPlan();

            plan.Day = day;

            if (day == 1)
            {
                plan.Activities.Add($"Travel to {destination.Name}");
                plan.Activities.Add($"Check in at {hotel?.HotelName}");

                var afternoon = activities.FirstOrDefault(x =>
                    x.TimeSlot == "Afternoon");

                if (afternoon != null)
                    plan.Activities.Add(afternoon.ActivityName);

                var evening = activities.FirstOrDefault(x =>
                    x.TimeSlot == "Evening");

                if (evening != null)
                    plan.Activities.Add(evening.ActivityName);

                plan.Activities.Add("Dinner at local restaurant");
            }
            else if (day == request.Days)
            {
                plan.Activities.Add("Breakfast");

                var morning = activities.Skip(index)
                    .FirstOrDefault(x => x.TimeSlot == "Morning");

                if (morning != null)
                {
                    plan.Activities.Add(morning.ActivityName);
                    index++;
                }

                plan.Activities.Add("Shopping");
                plan.Activities.Add("Hotel Check-out");
                plan.Activities.Add("Return Journey");
            }
            else
            {
                plan.Activities.Add("Breakfast");

                var morning = activities.Skip(index)
                    .FirstOrDefault(x => x.TimeSlot == "Morning");

                if (morning != null)
                {
                    plan.Activities.Add(morning.ActivityName);
                    index++;
                }

                var afternoon = activities.Skip(index)
                    .FirstOrDefault(x => x.TimeSlot == "Afternoon");

                if (afternoon != null)
                {
                    plan.Activities.Add(afternoon.ActivityName);
                    index++;
                }

                var evening = activities.Skip(index)
                    .FirstOrDefault(x => x.TimeSlot == "Evening");

                if (evening != null)
                {
                    plan.Activities.Add(evening.ActivityName);
                    index++;
                }

                plan.Activities.Add("Dinner");
            }

            response.Days.Add(plan);
        }

        return response;
    }
}