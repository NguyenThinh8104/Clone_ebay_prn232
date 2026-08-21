using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers;

[ApiController]
[Route("api/disputes")]
public class DisputeController : ControllerBase
{
    [HttpGet]
    public IActionResult GetDisputes() => StatusCode(501, "Not Implemented");

    [HttpPut("{id}/resolve")]
    public IActionResult ResolveDispute(int id) => StatusCode(501, "Not Implemented");
}
