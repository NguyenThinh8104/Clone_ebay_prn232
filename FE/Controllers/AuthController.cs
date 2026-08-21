using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers;

public class AuthController : Controller
{
    public IActionResult Login() => View();
    public IActionResult Register() => View();
}
