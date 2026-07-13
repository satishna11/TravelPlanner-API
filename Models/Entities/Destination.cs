using TravelAI.Models.Entities;

public class Destination
{
    public int DestinationId { get; set; }

    public string Name { get; set; } = "";

    public string City { get; set; } = "";

    public string Country { get; set; } = "";

    public string Description { get; set; } = "";

    public decimal AverageBudget { get; set; }

    public string ImageUrl { get; set; } = "";
    public int ViewCount { get; set; }
    public ICollection<Hotel>? Hotels { get; set; }

    public DestinationFeature? Feature { get; set; }
}