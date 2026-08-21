using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers;

public class MessageController : Controller
{
    public IActionResult Index() => View();
}
