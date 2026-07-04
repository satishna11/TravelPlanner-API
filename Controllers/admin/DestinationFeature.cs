using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAI.Data;
using TravelAI.Models.Entities;

namespace TravelAI.Controllers
{
    [Route("api/[controller]")]
    public class DestinationFeatureController : AdminBaseController
    {
        private readonly AppDbContext _context;

        public DestinationFeatureController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/DestinationFeature
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.DestinationFeatures
                .Include(x => x.Destination)
                .ToListAsync();

            return Success(data);
        }

        // GET: api/DestinationFeature/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var feature = await _context.DestinationFeatures
                .Include(x => x.Destination)
                .FirstOrDefaultAsync(x => x.DestinationFeatureId == id);

            if (feature == null)
                return NotFoundResponse("Feature not found.");

            return Success(feature);
        }

        // GET: api/DestinationFeature/destination/3
        [HttpGet("destination/{destinationId}")]
        public async Task<IActionResult> GetByDestination(int destinationId)
        {
            var feature = await _context.DestinationFeatures
                .FirstOrDefaultAsync(x => x.DestinationId == destinationId);

            if (feature == null)
                return NotFoundResponse("Feature not found.");

            return Success(feature);
        }

        // POST: api/DestinationFeature
        [HttpPost]
        public async Task<IActionResult> Create(DestinationFeature model)
        {
            var destination = await _context.Destinations
                .FindAsync(model.DestinationId);

            if (destination == null)
                return Fail("Destination not found.");

            var exists = await _context.DestinationFeatures
                .AnyAsync(x => x.DestinationId == model.DestinationId);

            if (exists)
                return Fail("Features already exist for this destination.");

            _context.DestinationFeatures.Add(model);

            await _context.SaveChangesAsync();

            return Success(new DestinationFeatureDto
            {
                DestinationFeatureId = model.DestinationFeatureId,
                DestinationId = model.DestinationId,
                Adventure = model.Adventure,
                Nature = model.Nature,
                Culture = model.Culture,
                Luxury = model.Luxury,
                Wildlife = model.Wildlife,
                Trekking = model.Trekking,
                Family = model.Family,
                Relaxation = model.Relaxation,
                Religious = model.Religious,
                NightLife = model.NightLife
            }, "Destination features created successfully.");
        }

        // PUT: api/DestinationFeature/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DestinationFeature model)
        {
            var feature = await _context.DestinationFeatures
                .FindAsync(id);

            if (feature == null)
                return NotFoundResponse("Feature not found.");

            feature.Adventure = model.Adventure;
            feature.Nature = model.Nature;
            feature.Culture = model.Culture;
            feature.Luxury = model.Luxury;
            feature.Wildlife = model.Wildlife;
            feature.Trekking = model.Trekking;
            feature.Family = model.Family;
            feature.Relaxation = model.Relaxation;
            feature.Religious = model.Religious;
            feature.NightLife = model.NightLife;

            await _context.SaveChangesAsync();

            return Success(feature, "Destination features updated successfully.");
        }

        // DELETE: api/DestinationFeature/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var feature = await _context.DestinationFeatures
                .FindAsync(id);

            if (feature == null)
                return NotFoundResponse("Feature not found.");

            _context.DestinationFeatures.Remove(feature);

            await _context.SaveChangesAsync();

            return Success(null, "Destination features deleted successfully.");
        }
    }
}