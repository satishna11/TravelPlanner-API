namespace TravelAI.Models.DTOs
{
    public class DestinationActivityDto
    {
        public int DestinationActivityId { get; set; }

        public int DestinationId { get; set; }

        public string DestinationName { get; set; } = string.Empty;

        public string ActivityName { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string TimeSlot { get; set; } = string.Empty;

        public decimal EstimatedCost { get; set; }

        public int DurationHours { get; set; }

        public string? ImageUrl { get; set; }
    }
}