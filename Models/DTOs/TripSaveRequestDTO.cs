namespace TravelAI.Models.DTOs
{
    public class TripSaveRequest
    {
        public TripRequest TripRequest { get; set; } = new();

        public int DestinationId { get; set; }

        public ItenaryResponse.ItineraryResponse Itinerary { get; set; } = new();
    }
}