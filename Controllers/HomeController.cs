using Microsoft.AspNetCore.Mvc;
using SlowInsurance.Models;
using System.Diagnostics;

namespace SlowInsurance.Controllers
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
    }
}