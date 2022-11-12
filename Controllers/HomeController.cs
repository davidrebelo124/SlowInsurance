using Microsoft.AspNetCore.Mvc;
using SlowInsurance.Models;

namespace SlowInsurance.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}