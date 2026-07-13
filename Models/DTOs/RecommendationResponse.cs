namespace TravelAI.Models.DTOs;

public class RecommendationResponse
{
    public string DestinationName { get; set; } = string.Empty;
    public double Similarity { get; set; }
}