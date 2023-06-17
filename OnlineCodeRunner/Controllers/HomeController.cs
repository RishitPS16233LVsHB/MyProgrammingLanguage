using Microsoft.AspNetCore.Mvc;

namespace OnlineCodeRunner.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
