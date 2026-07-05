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

        [Authorize(Roles = "User")]
        [HttpPost("generate-itinerary")]
        public async Task<IActionResult> GenerateItinerary([FromBody] TripRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _itineraryService.Generate(
                userId,
                request,
                request.DestinationId!.Value);

            return Success(result);
        }
    }
}