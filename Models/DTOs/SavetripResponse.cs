namespace TravelAI.Models.DTOs;

public class SavetripResponse
{
    public class SaveTripResponse
    {
        public int TripId { get; set; }
        public DateTime TravelDate { get; set; }
        public int Days { get; set; }
        public decimal Budget { get; set; }
        public int Travellers { get; set; }
        public string Transportation { get; set; } = string.Empty;
        public string HotelCategory { get; set; } = string.Empty;
    }
}