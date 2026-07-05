namespace TravelAI.Models.DTOs;

public class ItenaryResponse
{
    public class ItineraryResponse
    {
        public string Destination { get; set; } = "";
        public string HotelName { get; set; } = "";
        public string Transportation { get; set; } = "";
        public decimal EstimatedBudget { get; set; }

        public List<DayPlan> Days { get; set; } = new();
    }
}