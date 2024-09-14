using CodeAIReviewAPI.Models;
using CodeAIReviewAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeAIReviewAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewController : ControllerBase
    {
        private readonly IAiService _aiService;

        public ReviewController(IAiService aiService)
        {
            _aiService = aiService;
        }
        [HttpPost]
        public async Task<IActionResult> PostFileChanges([FromBody] List<FileChange> fileChanges)
        {
            // Get the Firebase token
            var userId = User.FindFirst("user_id")?.Value;
            if (userId == null) return Unauthorized();

            var response = await _aiService.ReviewCodeChanges(fileChanges);

            return Ok(response);
        }
    }
}
