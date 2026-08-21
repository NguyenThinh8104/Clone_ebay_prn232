using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    [HttpPost("register")]
    public IActionResult Register() => StatusCode(501, "Not Implemented");

    [HttpPost("login")]
    public IActionResult Login() => StatusCode(501, "Not Implemented");

    [HttpPost("refresh-token")]
    public IActionResult RefreshToken() => StatusCode(501, "Not Implemented");

    [HttpPost("logout")]
    public IActionResult Logout() => StatusCode(501, "Not Implemented");
}
