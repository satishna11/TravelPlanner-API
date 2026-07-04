using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelAI.Models.Entities;
namespace TravelAI.Models.Entities
{
    public class TripDestination
    {
        [Key]
        public int TripDestinationId { get; set; }

        [ForeignKey("Trip")]
        public int TripId { get; set; }

        public Trip? Trip { get; set; }

        [ForeignKey("Destination")]
        public int DestinationId { get; set; }

        public Destination? Destination { get; set; }

        public int Sequence { get; set; }
    }
}