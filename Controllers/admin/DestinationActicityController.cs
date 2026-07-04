using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAI.Data;
using TravelAI.Models.Entities;

namespace TravelAI.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    public class DestinationActivityController : AdminBaseController
    {
        private readonly AppDbContext _context;

        public DestinationActivityController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/admin/DestinationActivity
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var activities = await _context.DestinationActivity
                .Include(x => x.Destination)
                .ToListAsync();

            return Success(activities);
        }

        // GET: api/admin/DestinationActivity/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var activity = await _context.DestinationActivity
                .Include(x => x.Destination)
                .FirstOrDefaultAsync(x => x.DestinationActivityId == id);

            if (activity == null)
                return NotFoundResponse("Activity not found.");

            return Success(activity);
        }

        // GET: api/admin/DestinationActivity/destination/1
        [HttpGet("destination/{destinationId}")]
        public async Task<IActionResult> GetByDestination(int destinationId)
        {
            var activities = await _context.DestinationActivity
                .Where(x => x.DestinationId == destinationId)
                .ToListAsync();

            return Success(activities);
        }

        // POST: api/admin/DestinationActivity
        [HttpPost]
        public async Task<IActionResult> Create(DestinationActivity activity)
        {
            var destination = await _context.Destinations
                .FindAsync(activity.DestinationId);

            if (destination == null)
                return Fail("Destination not found.");

            _context.DestinationActivity.Add(activity);

            await _context.SaveChangesAsync();

            return Success(activity, "Activity created successfully.");
        }

        // PUT: api/admin/DestinationActivity/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DestinationActivity activity)
        {
            var existing = await _context.DestinationActivity
                .FindAsync(id);

            if (existing == null)
                return NotFoundResponse("Activity not found.");

            existing.ActivityName = activity.ActivityName;
            existing.TimeSlot = activity.TimeSlot;
            existing.Category = activity.Category;
            existing.EstimatedCost = activity.EstimatedCost;
            existing.DurationHours = activity.DurationHours;
            existing.ImageUrl = activity.ImageUrl;
            existing.DestinationId = activity.DestinationId;

            await _context.SaveChangesAsync();

            return Success(existing, "Activity updated successfully.");
        }

        // DELETE: api/admin/DestinationActivity/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var activity = await _context.DestinationActivity
                .FindAsync(id);

            if (activity == null)
                return NotFoundResponse("Activity not found.");

            _context.DestinationActivity.Remove(activity);

            await _context.SaveChangesAsync();

            return Success(null, "Activity deleted successfully.");
        }
    }
}