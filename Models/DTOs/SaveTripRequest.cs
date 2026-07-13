using System.ComponentModel.DataAnnotations;

namespace TravelAI.Models.DTOs
{
    public class SaveTripRequest
    {
        [Required]
        public DateTime TravelDate { get; set; }

        [Required]
        public int Days { get; set; }

        [Required]
        public decimal Budget { get; set; }

        [Required]
        public int Travellers { get; set; }

        public string? Transportation { get; set; }

        public string? HotelCategory { get; set; }

        [Required]
        public int DestinationId { get; set; }


        // Generated itinerary data
        public List<SaveItineraryRequest> Itineraries { get; set; } = new();
    }


    public class SaveItineraryRequest
    {
        public int DayNumber { get; set; }

        public string? Morning { get; set; }

        public string? Afternoon { get; set; }

        public string? Evening { get; set; }

        public decimal EstimatedCost { get; set; }
    }
}