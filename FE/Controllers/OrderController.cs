using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers;

public class OrderController : Controller
{
    public IActionResult Index() => View();
    public IActionResult Details(int id) => View();
}
