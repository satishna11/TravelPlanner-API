using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelAI.Models.DTOs;
using TravelAI.Services;

namespace TravelAI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User")]
    public class ItinenaryController : BaseController
    {
        private readonly ItineraryService _itineraryService;

        public ItinenaryController(ItineraryService itineraryService)
        {
            _itineraryService = itineraryService;
        }

        [HttpPost("generate-itinerary")]
        public async Task<IActionResult> GenerateItinerary([FromBody] TripRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (request.TravelDate.Date < DateTime.Today)
                return Fail("Travel date cannot be in the past.");

            if (request.Days <= 0)
                return Fail("Days must be greater than zero.");

            if (request.Budget <= 0)
                return Fail("Budget must be greater than zero.");

            if (request.Travellers <= 0)
                return Fail("Travellers must be greater than zero.");
            // Check request
            if (request == null)
            {
                return BadRequest("Request cannot be null.");
            }

            // Check DestinationId
            if (!request.DestinationId.HasValue)
            {
                return BadRequest("DestinationId is required.");
            }

            // Get logged-in user
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("Invalid User ID.");
            }

            // Generate itinerary
            try
            {
                var result = await _itineraryService.Generate(
                    userId,
                    request,
                    request.DestinationId.Value);

                return Success(result, "Itinerary generated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
            return Success( "Itinerary generated successfully.");
        }
    }
}