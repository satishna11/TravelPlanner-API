using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TravelAI.Models.DTOs
{
    public class HotelDto
    {
        [Required]
        public string HotelName { get; set; } = string.Empty;

        [Required]
        public decimal PricePerNight { get; set; }

        [Required]
        public double Rating { get; set; }

        [Required]
        public string Category { get; set; } = string.Empty;

        public string Facilities { get; set; } = string.Empty;
        
        
        [Required]
        public int DestinationId { get; set; }

        // Image uploaded by admin
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;
    }
}