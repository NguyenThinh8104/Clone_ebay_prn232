using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers;

[ApiController]
[Route("api/inventory")]
public class InventoryController : ControllerBase
{
    [HttpGet("{productId}")]
    public IActionResult GetInventory(int productId) => StatusCode(501, "Not Implemented");

    [HttpPut("{productId}")]
    public IActionResult UpdateInventory(int productId) => StatusCode(501, "Not Implemented");
}
