using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAI.Data;
using TravelAI.Models.Entities;

namespace TravelAI.Controllers
{
    
    [Route("api/[controller]")]
    public class DestinationController : AdminBaseController
    {
        
        private readonly AppDbContext _context;

        public DestinationController(AppDbContext context)
        {
            _context = context;
        }

        // GET ALL
  
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var destinations = await _context.Destinations
                .Include(d => d.Hotels)
                .ToListAsync();

            return Success(destinations);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var destination = await _context.Destinations
                .Include(d => d.Hotels)
                .FirstOrDefaultAsync(d => d.DestinationId == id);

            if (destination == null)
                return NotFoundResponse("Destination not found");

            return Success(destination);
        }

        // CREATE
        [HttpPost]
        public async Task<IActionResult> Create(Destination destination)
        {
            _context.Destinations.Add(destination);
            await _context.SaveChangesAsync();

            return Success(destination, "Destination created");
        }

        // UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Destination destination)
        {
            var existing = await _context.Destinations.FindAsync(id);

            if (existing == null)
                return NotFoundResponse("Destination not found");

            existing.Name = destination.Name;
            existing.City = destination.City;
            existing.Country = destination.Country;
            existing.Description = destination.Description;
            existing.AverageBudget = destination.AverageBudget;
            existing.ImageUrl = destination.ImageUrl;

            await _context.SaveChangesAsync();

            return Success(existing, "Destination updated");
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var destination = await _context.Destinations.FindAsync(id);

            if (destination == null)
                return NotFoundResponse("Destination not found");

            _context.Destinations.Remove(destination);
            await _context.SaveChangesAsync();

            return Success(null, "Destination deleted");
        }
    }
}