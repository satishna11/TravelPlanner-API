using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAI.Models.DTOs;
using TravelAI.Services;

namespace TravelAI.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User")]
    public class RecommendationController : BaseController
    {
        private readonly CosineSimilarityService _cosineService;

        public RecommendationController(CosineSimilarityService cosineService)
        {
            _cosineService = cosineService;
        }

        [HttpPost("recommend")]
        public async Task<IActionResult> Recommend([FromBody] RecommendationRequest request)
        {
            var result = await _cosineService.RecommendDestinations(request);

            return Success(result);
        }
    }
}