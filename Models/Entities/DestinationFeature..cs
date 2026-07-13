using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class DestinationFeature
{
    [Key]
    public int DestinationFeatureId { get; set; }

    [ForeignKey(nameof(Destination))]
    public int DestinationId { get; set; }

    public Destination? Destination { get; set; }

    public int Adventure { get; set; }

    public int Nature { get; set; }

    public int Wildlife { get; set; }

    public int Religious { get; set; }

    public int Culture { get; set; }

    public int Luxury { get; set; }

    public int Trekking { get; set; }
}