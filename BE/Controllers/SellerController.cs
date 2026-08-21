using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers;

[ApiController]
[Route("api/seller")]
public class SellerController : ControllerBase
{
    [HttpPost("apply")]
    public IActionResult ApplySeller() => StatusCode(501, "Not Implemented");

    [HttpGet("verification-status")]
    public IActionResult GetVerificationStatus() => StatusCode(501, "Not Implemented");
}

[ApiController]
[Route("api/admin/seller")]
public class AdminSellerController : ControllerBase
{
    [HttpPut("{id}/approve")]
    public IActionResult ApproveSeller(int id) => StatusCode(501, "Not Implemented");

    [HttpPut("{id}/reject")]
    public IActionResult RejectSeller(int id) => StatusCode(501, "Not Implemented");
}
