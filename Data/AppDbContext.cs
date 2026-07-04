using Microsoft.EntityFrameworkCore;
using TravelAI.Models.Entities;

namespace TravelAI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Destination> Destinations { get; set; }

        public DbSet<DestinationFeature> DestinationFeatures { get; set; }
        public DbSet<DestinationActivity> DestinationActivity { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

        public DbSet<Trip> Trips { get; set; }

        public DbSet<TripDestination> TripDestinations { get; set; }

        public DbSet<Itinerary> Itineraries { get; set; }
    }
}