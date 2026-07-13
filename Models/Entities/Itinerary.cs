using System.ComponentModel.DataAnnotations.Schema;
using TravelAI.Models.Entities;

public class Itinerary
{
    public int ItineraryId { get; set; }
    [ForeignKey("Trip")]
    public int TripId { get; set; }
    public Trip? Trip { get; set; }   
    public int DestinationId { get; set; }

    public int DayNumber { get; set; }

    public string Morning { get; set; } = "";

    public string Afternoon { get; set; } = "";

    public string Evening { get; set; } = "";

    public decimal EstimatedCost { get; set; }
}