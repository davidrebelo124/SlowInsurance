using Microsoft.AspNetCore.Mvc;

namespace SlowInsurance.Controllers
{
    public class UserController : Controller
    {
        public IActionResult SignIn()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        //public IActionResult Register()
        //{

        //}
    }
}
