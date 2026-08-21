using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    [HttpGet]
    public IActionResult GetOrders() => StatusCode(501, "Not Implemented");

    [HttpGet("{id}")]
    public IActionResult GetOrderById(int id) => StatusCode(501, "Not Implemented");

    [HttpPut("{id}/confirm")]
    public IActionResult ConfirmOrder(int id) => StatusCode(501, "Not Implemented");

    [HttpPut("{id}/status")]
    public IActionResult UpdateOrderStatus(int id) => StatusCode(501, "Not Implemented");

    [HttpPost("{id}/shipping-label")]
    public IActionResult CreateShippingLabel(int id) => StatusCode(501, "Not Implemented");

    [HttpGet("{id}/payment")]
    public IActionResult GetPaymentStatus(int id) => StatusCode(501, "Not Implemented");
}
