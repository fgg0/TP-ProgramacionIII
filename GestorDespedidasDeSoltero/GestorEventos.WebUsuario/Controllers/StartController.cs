using Microsoft.AspNetCore.Mvc;

public class StartController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}