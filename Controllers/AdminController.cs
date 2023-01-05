using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SlowInsurance.Entity;
using SlowInsurance.Repo;

namespace SlowInsurance.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        public AdminController()
        {
            
        }

        [HttpGet]
        public IActionResult MainPage() => View();

    }
}
