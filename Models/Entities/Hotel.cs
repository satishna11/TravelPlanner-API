using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelAI.Models;
using TravelAI.Models.Entities;

namespace TravelAI.Models.Entities
{
    public class Hotel
    {
        [Key]
        public int HotelId { get; set; }

        [Required]
        public string HotelName { get; set; } = string.Empty;

        public decimal PricePerNight { get; set; }

        public double Rating { get; set; }

        public string Category { get; set; } = string.Empty;

        public string Facilities { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        [ForeignKey("Destination")]
        public int DestinationId { get; set; }

        public Destination? Destination { get; set; }
    }
}