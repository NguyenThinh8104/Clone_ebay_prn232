using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers;

[ApiController]
[Route("api/reviews")]
public class ReviewController : ControllerBase
{
    [HttpGet]
    public IActionResult GetReviews() => StatusCode(501, "Not Implemented");

    [HttpPost("{id}/reply")]
    public IActionResult ReplyReview(int id) => StatusCode(501, "Not Implemented");
}

[ApiController]
[Route("api/feedback")]
public class FeedbackController : ControllerBase
{
    [HttpGet("{sellerId}")]
    public IActionResult GetFeedback(int sellerId) => StatusCode(501, "Not Implemented");
}
