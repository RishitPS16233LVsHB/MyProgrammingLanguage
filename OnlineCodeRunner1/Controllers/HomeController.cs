using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineCodeRunner1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Geralt_Roger_Eric_Du_Haute_Bellegarde;

namespace OnlineCodeRunner1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string code, string arguments="1")
        {
            MyProgram.RunProgramOnText(code, arguments.Split(','));
            ViewData["output"] = MyProgram.OutputStream;
            ViewData["code"] = code;
            ViewData["args"] = arguments;
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
