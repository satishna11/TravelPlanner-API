namespace TravelAI.Models.DTOs;

public class RecommendationResult
{
    
        public int DestinationId { get; set; }

        public string DestinationName { get; set; } = "";

        public double Similarity { get; set; }
    
}