using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers;

[ApiController]
[Route("api/coupons")]
public class CouponController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateCoupon() => StatusCode(501, "Not Implemented");

    [HttpGet]
    public IActionResult GetCoupons() => StatusCode(501, "Not Implemented");

    [HttpPut("{id}")]
    public IActionResult UpdateCoupon(int id) => StatusCode(501, "Not Implemented");

    [HttpDelete("{id}")]
    public IActionResult DeleteCoupon(int id) => StatusCode(501, "Not Implemented");

    [HttpGet("validate")]
    public IActionResult ValidateCoupon() => StatusCode(501, "Not Implemented");

    [HttpGet("{id}/usage-stats")]
    public IActionResult GetUsageStats(int id) => StatusCode(501, "Not Implemented");
}
