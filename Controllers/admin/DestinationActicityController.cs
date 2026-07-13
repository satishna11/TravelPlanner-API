using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAI.Data;
using TravelAI.Models.DTOs;
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
                .Select(x => new DestinationActivityDto
                {
                    DestinationActivityId = x.DestinationActivityId,
                    DestinationId = x.DestinationId,
                    DestinationName = x.Destination.Name,
                    ActivityName = x.ActivityName,
                    Category = x.Category,
                    TimeSlot = x.TimeSlot,
                    EstimatedCost = x.EstimatedCost,
                    DurationHours = x.DurationHours,
                    ImageUrl = x.ImageUrl
                })
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
                .Select(x => new DestinationActivityDto
                {
                    DestinationActivityId = x.DestinationActivityId,
                    DestinationId = x.DestinationId,
                    ActivityName = x.ActivityName,
                    Category = x.Category,
                    TimeSlot = x.TimeSlot,
                    EstimatedCost = x.EstimatedCost,
                    DurationHours = x.DurationHours,
                    ImageUrl = x.ImageUrl
                })
                .ToListAsync();

            return Success(activities);
        }
        // POST: api/admin/DestinationActivity
        [HttpPost]
        public async Task<IActionResult> Create(DestinationActivityDto dto)
        {
            var destination = await _context.Destinations
                .FindAsync(dto.DestinationId);

            if (destination == null)
                return Fail("Destination not found.");

            var activity = new DestinationActivity
            {
                DestinationId = dto.DestinationId,
                ActivityName = dto.ActivityName,
                Category = dto.Category,
                TimeSlot = dto.TimeSlot,
                EstimatedCost = dto.EstimatedCost,
                DurationHours = dto.DurationHours,
                ImageUrl = dto.ImageUrl
            };

            _context.DestinationActivity.Add(activity);

            await _context.SaveChangesAsync();

            return Success(activity, "Activity created successfully.");
        }
        // PUT: api/admin/DestinationActivity/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DestinationActivityDto dto)
        {
            var activity = await _context.DestinationActivity
                .FindAsync(id);

            if (activity == null)
                return NotFoundResponse("Activity not found.");

            activity.DestinationId = dto.DestinationId;
            activity.ActivityName = dto.ActivityName;
            activity.Category = dto.Category;
            activity.TimeSlot = dto.TimeSlot;
            activity.EstimatedCost = dto.EstimatedCost;
            activity.DurationHours = dto.DurationHours;
            activity.ImageUrl = dto.ImageUrl;

            await _context.SaveChangesAsync();

            return Success(activity, "Activity updated successfully.");
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