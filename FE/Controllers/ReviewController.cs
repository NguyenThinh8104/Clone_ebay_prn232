using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers;

public class ReviewController : Controller
{
    public IActionResult Index() => View();
    public IActionResult Returns() => View();
    public IActionResult Disputes() => View();
}
