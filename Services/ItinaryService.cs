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

        // ============================
        // Destination
        // ============================

        var destination = await _context.Destinations
            .FirstOrDefaultAsync(x =>
                x.DestinationId == destinationId);


        if(destination == null)
        {
            throw new Exception("Destination not found.");
        }



        // ============================
        // Hotel Selection
        // ============================

        var hotel = await _context.Hotels
            .Where(x =>
                x.DestinationId == destinationId &&
                x.Category == request.HotelCategory)
            .OrderByDescending(x => x.Rating)
            .FirstOrDefaultAsync();


        // fallback hotel

        if(hotel == null)
        {
            hotel = await _context.Hotels
                .Where(x => x.DestinationId == destinationId)
                .OrderByDescending(x => x.Rating)
                .FirstOrDefaultAsync();
        }



        // ============================
        // Activities
        // ============================

        var activities = await _context.DestinationActivity
            .Where(x =>
                x.DestinationId == destinationId)
            .OrderBy(x=>x.TimeSlot)
            .ToListAsync();



        if(!activities.Any())
        {
            throw new Exception(
                "No activities found for this destination."
            );
        }



        // ============================
        // Budget Calculation
        // Transport excluded
        // ============================


        decimal hotelCost = 0;


        if(hotel != null)
        {
            hotelCost =
                hotel.PricePerNight *
                request.Days;
        }



        decimal activityCost =
            activities.Sum(x=>x.EstimatedCost)
            *
            request.Travellers;



        decimal totalBudget =
            hotelCost +
            activityCost;



        // ============================
        // Response
        // ============================


        var response =
            new ItenaryResponse.ItineraryResponse
            {

                Destination = destination.Name,

                HotelName =
                    hotel?.HotelName ?? "No hotel available",

                Transportation =
                    request.Transportation,

                EstimatedBudget =
                    totalBudget

            };



        int activityIndex = 0;



        // ============================
        // Generate Days
        // ============================


        for(int day=1; day<=request.Days; day++)
        {


            var dayPlan = new DayPlan
            {
                Day = day
            };



            // ========================
            // First Day
            // ========================


            if(day == 1)
            {

                dayPlan.Activities.Add(
                    $"Travel to {destination.Name} by {request.Transportation}"
                );


                dayPlan.Activities.Add(
                    $"Check-in at {hotel?.HotelName ?? "recommended hotel"}"
                );



                AddTravelTypeActivity(
                    dayPlan,
                    request.TravelType
                );


                var evening =
                    activities
                    .FirstOrDefault(
                        x=>x.TimeSlot=="Evening"
                    );


                if(evening!=null)
                {
                    dayPlan.Activities.Add(
                        evening.ActivityName
                    );
                }


                dayPlan.Activities.Add(
                    "Dinner at local restaurant"
                );

            }



            // ========================
            // Last Day
            // ========================

            else if(day == request.Days)
            {

                dayPlan.Activities.Add(
                    "Breakfast"
                );


                var activity =
                    GetNextActivity(
                        activities,
                        ref activityIndex,
                        "Morning"
                    );


                if(activity!=null)
                {
                    dayPlan.Activities.Add(
                        activity.ActivityName
                    );
                }



                dayPlan.Activities.Add(
                    "Shopping for souvenirs"
                );


                dayPlan.Activities.Add(
                    "Hotel check-out"
                );


                dayPlan.Activities.Add(
                    "Return journey"
                );


                AddTravelTypeActivity(
                    dayPlan,
                    request.TravelType
                );

            }



            // ========================
            // Middle Days
            // ========================

            else
            {

                dayPlan.Activities.Add(
                    "Breakfast"
                );



                var morning =
                    GetNextActivity(
                        activities,
                        ref activityIndex,
                        "Morning"
                    );


                if(morning!=null)
                {
                    dayPlan.Activities.Add(
                        morning.ActivityName
                    );
                }



                var afternoon =
                    GetNextActivity(
                        activities,
                        ref activityIndex,
                        "Afternoon"
                    );


                if(afternoon!=null)
                {
                    dayPlan.Activities.Add(
                        afternoon.ActivityName
                    );
                }



                var evening =
                    GetNextActivity(
                        activities,
                        ref activityIndex,
                        "Evening"
                    );


                if(evening!=null)
                {
                    dayPlan.Activities.Add(
                        evening.ActivityName
                    );
                }



                AddTravelTypeActivity(
                    dayPlan,
                    request.TravelType
                );


                dayPlan.Activities.Add(
                    "Dinner"
                );

            }



            response.Days.Add(dayPlan);

        }



        return response;

    }



    // ==================================
    // Get activity by time
    // ==================================

    private DestinationActivity? GetNextActivity(
        List<DestinationActivity> activities,
        ref int index,
        string time)
    {

        while(index < activities.Count)
        {

            var activity = activities[index];

            index++;


            if(activity.TimeSlot == time)
            {
                return activity;
            }

        }


        return null;
    }




    // ==================================
    // Travel Type Logic
    // ==================================

    private void AddTravelTypeActivity(
        DayPlan plan,
        string travelType)
    {

        switch(travelType)
        {

            case "Solo":

                plan.Activities.Add(
                    "Explore local places independently and enjoy personal time."
                );

                break;



            case "Couple":

                plan.Activities.Add(
                    "Enjoy romantic viewpoints and memorable moments together."
                );

                break;



            case "Family":

                plan.Activities.Add(
                    "Visit family-friendly attractions and spend quality time."
                );

                break;



            case "Friends":

                plan.Activities.Add(
                    "Enjoy adventure activities and group experiences."
                );

                break;

        }

    }

}