using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers;

[ApiController]
[Route("api/returns")]
public class ReturnController : ControllerBase
{
    [HttpGet]
    public IActionResult GetReturns() => StatusCode(501, "Not Implemented");

    [HttpPut("{id}/status")]
    public IActionResult UpdateReturnStatus(int id) => StatusCode(501, "Not Implemented");
}
