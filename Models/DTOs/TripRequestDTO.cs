using System.ComponentModel.DataAnnotations.Schema;

public class TripRequest
{
 
    public int? DestinationId { get; set; }
    public DateTime TravelDate { get; set; }

    public int Days { get; set; }

    public decimal Budget { get; set; }

    public int Travellers { get; set; }

    public string Transportation { get; set; } = "";
    public string TravelType { get; set; } = "";
    public string HotelCategory { get; set; } = "";

    public int Adventure { get; set; }

    public int Nature { get; set; }

    public int Culture { get; set; }

    public int Luxury { get; set; }

    public int Wildlife { get; set; }

    public int Trekking { get; set; }

    public int Family { get; set; }

    public int Relaxation { get; set; }

    public int Religious { get; set; }

    public int NightLife { get; set; }
}