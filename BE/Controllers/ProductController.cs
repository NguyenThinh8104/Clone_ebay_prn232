using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateProduct() => StatusCode(501, "Not Implemented");

    [HttpGet]
    public IActionResult GetProducts() => StatusCode(501, "Not Implemented");

    [HttpGet("{id}")]
    public IActionResult GetProductById(int id) => StatusCode(501, "Not Implemented");

    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id) => StatusCode(501, "Not Implemented");

    [HttpPatch("{id}/hide")]
    public IActionResult HideProduct(int id) => StatusCode(501, "Not Implemented");

    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id) => StatusCode(501, "Not Implemented");

    [HttpGet("{id}/bids")]
    public IActionResult GetProductBids(int id) => StatusCode(501, "Not Implemented");

    [HttpPost("{id}/close-auction")]
    public IActionResult CloseAuction(int id) => StatusCode(501, "Not Implemented");
}
