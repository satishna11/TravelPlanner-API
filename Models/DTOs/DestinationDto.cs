using Microsoft.AspNetCore.Http;

namespace TravelAI.Models.DTOs
{
    public class DestinationDto
    {
        public int DestinationId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal AverageBudget { get; set; }

        // File uploaded from React
        public IFormFile? Image { get; set; }

        // Saved image path
        public string? ImageUrl { get; set; }

        public int ViewCount { get; set; }
    }
}