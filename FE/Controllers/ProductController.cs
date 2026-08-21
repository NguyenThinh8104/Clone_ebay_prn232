using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers;

public class ProductController : Controller
{
    public IActionResult Index() => View();
    public IActionResult Create() => View();
    public IActionResult Edit(int id) => View();
    public IActionResult Inventory() => View();
    public IActionResult Bids(int id) => View();
}
