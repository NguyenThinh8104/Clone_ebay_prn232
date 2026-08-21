using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers;

[ApiController]
[Route("api/shipping")]
public class ShippingController : ControllerBase
{
    [HttpGet("{orderId}")]
    public IActionResult GetShippingInfo(int orderId) => StatusCode(501, "Not Implemented");
}
