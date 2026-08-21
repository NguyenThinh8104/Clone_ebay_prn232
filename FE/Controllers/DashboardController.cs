using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers;

public class DashboardController : Controller
{
    public IActionResult Index() => View();
}
