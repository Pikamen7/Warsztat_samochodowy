using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

public class ServiceOrdersController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}