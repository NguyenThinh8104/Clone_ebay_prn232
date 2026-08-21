using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers;

public class CouponController : Controller
{
    public IActionResult Index() => View();
    public IActionResult Create() => View();
}
