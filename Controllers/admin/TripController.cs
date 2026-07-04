using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAI.Data;
using TravelAI.Models.Entities;

namespace TravelAI.Controllers
{
    [Route("api/[controller]")]
    public class TripController : AdminBaseController
    {
        private readonly AppDbContext _context;

        public TripController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var trips = await _context.Trips
                .Include(t => t.TripDestinations)
                .ToListAsync();

            return Success(trips);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var trip = await _context.Trips
                .Include(t => t.TripDestinations)
                .FirstOrDefaultAsync(t => t.TripId == id);

            if (trip == null)
                return NotFoundResponse("Trip not found");

            return Success(trip);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Trip trip)
        {
            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();

            return Success(trip, "Trip created");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var trip = await _context.Trips.FindAsync(id);

            if (trip == null)
                return NotFoundResponse("Trip not found");

            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();

            return Success(null, "Trip deleted");
        }
    }
}