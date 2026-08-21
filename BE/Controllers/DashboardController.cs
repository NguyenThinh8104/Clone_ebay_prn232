using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    [HttpGet("summary")]
    public IActionResult GetSummary() => StatusCode(501, "Not Implemented");

    [HttpGet("top-products")]
    public IActionResult GetTopProducts() => StatusCode(501, "Not Implemented");

    [HttpGet("revenue-chart")]
    public IActionResult GetRevenueChart() => StatusCode(501, "Not Implemented");

    [HttpGet("performance")]
    public IActionResult GetPerformance() => StatusCode(501, "Not Implemented");
}
