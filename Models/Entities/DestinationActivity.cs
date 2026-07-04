using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAI.Models.Entities
{
    public class DestinationActivity
    {
        [Key]
        public int DestinationActivityId { get; set; }

        [ForeignKey(nameof(Destination))]
        public int DestinationId { get; set; }

        public Destination? Destination { get; set; }

        [Required]
        public string ActivityName { get; set; } = "";

        // Morning Afternoon Evening
        public string TimeSlot { get; set; } = "";

        // Nature Adventure Culture Religious etc
        public string Category { get; set; } = "";

        public decimal EstimatedCost { get; set; }

        // Hours
        public int DurationHours { get; set; }

        public string ImageUrl { get; set; } = "";
    }
}