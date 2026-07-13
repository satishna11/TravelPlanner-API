using TravelAI.Models.Entities;

namespace TravelAI.Models.Entities
{
    public class Trip
    {
        public int TripId { get; set; }

        public int UserId { get; set; }

        public DateTime TravelDate { get; set; }

        public int Days { get; set; }

        public decimal Budget { get; set; }

        public int Travellers { get; set; }

        public string Transportation { get; set; } = "";

        public string HotelCategory { get; set; } = "";


        // Navigation properties
        public ICollection<TripDestination>? TripDestinations { get; set; }

        public ICollection<Itinerary>? Itineraries { get; set; }
    }
}