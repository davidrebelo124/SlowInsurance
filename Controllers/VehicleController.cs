using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SlowInsurance.Entity;
using SlowInsurance.Models;
using SlowInsurance.Repo;

namespace SlowInsurance.Controllers
{
    [Authorize]
    public class VehicleController : Controller
    {
        private readonly InsuranceDbContext context;

        public VehicleController(InsuranceDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult ListVehicles()
        {
            var vehicles = context.Users.First(u => u.UserName == User.Identity.Name).Vehicles;
            if(vehicles == null)
                return View(null);
            var model = vehicles.Select(v => new VehicleViewModel
            {
                Id = v.Id,
                VehicleType = v.VehicleType,
                Model = v.Model,
                RegistrationDate = v.RegistrationDate,
                Plate = v.Plate,
                AdhesionDate = v.AdhesionDate
            });
            return View(model);
        }

        [HttpGet]
        public IActionResult AddVehicle()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> AddVehicle(AddVehicleViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {

        //    }

        //    return View(model);
        //}
    }
}
