using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers;

[ApiController]
[Route("api/store")]
public class StoreController : ControllerBase
{
    [HttpGet("{sellerId}")]
    public IActionResult GetPublicStoreProfile(int sellerId) => StatusCode(501, "Not Implemented");

    [HttpPut]
    public IActionResult UpdateStoreProfile() => StatusCode(501, "Not Implemented");
}
