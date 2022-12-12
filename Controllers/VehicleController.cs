using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlowInsurance.Entity;
using SlowInsurance.Models;
using SlowInsurance.Models.Vehicle;
using SlowInsurance.Repo;
using System.Text;
using System.Text.Json;

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
            var vehicles = context.Users.Where(u => u.UserName == User.Identity.Name).Include(u => u.Vehicles).First().Vehicles.ToList();
            if(vehicles == null)
                return View();

            var model = vehicles.Select(v => new VehicleModel
            {
                Id = v.Id,
                VehicleType = Enum.Parse<VehicleType>(v.VehicleType),
                Model = v.Model,
                RegistrationDate = DateTime.Parse(v.RegistrationDate),
                Plate = v.Plate,
                AdhesionDate = DateTime.Parse(v.AdhesionDate),
            });
            return View(model);
        }

        [HttpGet]
        public IActionResult AddVehicle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddVehicle(AddVehicleModel modell)
        {
            if (!ModelState.IsValid)
                return View(modell);

            if (modell.RegistrationDate > DateTime.Now.AddYears(-18))
            {
                ModelState.AddModelError("", "Not a valid date");
                return View(modell);
            }

            var vehicles = context.Users.First(u => u.UserName == User.Identity.Name).Vehicles;
            var vehicle = new VehicleEntity
            {
                AdhesionDate = DateTime.Now.ToShortDateString(),
                Model = modell.Model,
                RegistrationDate = modell.RegistrationDate.ToShortDateString(),
                Plate = modell.Plate,
                VehicleType = modell.VehicleType.ToString(),
            };

            TempData["Vehicle"] = Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes(modell));
            return RedirectToAction("AddInvoiceWithVehicle", "Invoice");
        }
    }
}
