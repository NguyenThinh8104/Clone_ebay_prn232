using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers;

public class SellerController : Controller
{
    public IActionResult Apply() => View();
    public IActionResult Status() => View();
    public IActionResult StoreProfile() => View();
}
