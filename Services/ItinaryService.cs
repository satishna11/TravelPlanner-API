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

    public async Task<ItenaryResponse.ItineraryResponse> Generate(
        int userId,
        TripRequest request,
        int destinationId)
    {
        // Get destination
        var destination = await _context.Destinations
            .FirstOrDefaultAsync(x => x.DestinationId == destinationId);

        if (destination == null)
        {
            throw new Exception("Destination not found.");
        }


        // Select hotel according to category
        var hotel = await _context.Hotels
            .Where(x =>
                x.DestinationId == destinationId &&
                x.Category == request.HotelCategory)
            .OrderByDescending(x => x.Rating)
            .FirstOrDefaultAsync();


        // If category hotel not found, select highest rated hotel
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


        if (!activities.Any())
        {
            throw new Exception("No activities available for this destination.");
        }


        // Create response only
        // Nothing saved here
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


            // First day
            if (day == 1)
            {
                morningActivity = $"Travel to {destination.Name}";

                afternoonActivity =
                    $"Check in at {hotel?.HotelName ?? "recommended hotel"}";


                plan.Activities.Add(morningActivity);
                plan.Activities.Add(afternoonActivity);


                var evening = activities
                    .FirstOrDefault(x => x.TimeSlot == "Evening");


                if (evening != null)
                {
                    eveningActivity = evening.ActivityName;
                    plan.Activities.Add(eveningActivity);
                }


                plan.Activities.Add(
                    "Dinner at local restaurant"
                );
            }


            // Last day
            else if (day == request.Days)
            {
                plan.Activities.Add("Breakfast");


                var morning = activities
                    .Skip(index)
                    .FirstOrDefault(x =>
                        x.TimeSlot == "Morning");


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


            // Middle days
            else
            {
                plan.Activities.Add("Breakfast");


                var morning = activities
                    .Skip(index)
                    .FirstOrDefault(x =>
                        x.TimeSlot == "Morning");


                if (morning != null)
                {
                    morningActivity = morning.ActivityName;
                    plan.Activities.Add(morningActivity);
                    index++;
                }



                var afternoon = activities
                    .Skip(index)
                    .FirstOrDefault(x =>
                        x.TimeSlot == "Afternoon");


                if (afternoon != null)
                {
                    afternoonActivity = afternoon.ActivityName;
                    plan.Activities.Add(afternoonActivity);
                    index++;
                }



                var evening = activities
                    .Skip(index)
                    .FirstOrDefault(x =>
                        x.TimeSlot == "Evening");


                if (evening != null)
                {
                    eveningActivity = evening.ActivityName;
                    plan.Activities.Add(eveningActivity);
                    index++;
                }


                plan.Activities.Add("Dinner");
            }



            // Only add to response
            // Database saving happens after Save Trip button
            response.Days.Add(plan);
        }



        // No SaveChangesAsync()
        // No database insert


        return response;
    }
}