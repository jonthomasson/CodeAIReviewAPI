using CodeAIReviewAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeAIReviewAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewController : ControllerBase
    {
        [HttpPost]
        public IActionResult PostFileChanges([FromBody] List<FileChange> fileChanges)
        {
            // Get the Firebase token
            var userId = User.FindFirst("user_id")?.Value;
            if (userId == null) return Unauthorized();

            // Your logic for processing file changes and returning AI suggestions
            return Ok("Success");
        }
    }
}
