using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlowInsurance.Entity;
using SlowInsurance.Models.Accident;
using SlowInsurance.Models.Vehicle;
using SlowInsurance.Repo;

namespace SlowInsurance.Controllers
{
    [Authorize]
    public class AccidentController : Controller
    {
        private readonly InsuranceDbContext context;

        public AccidentController(InsuranceDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult ListAccidents()
        {
            var accidents = context.Accident.Select(a => new AccidentModel
            {
                Location = a.Location,
                Description = a.Description,
                Date = DateTime.Parse(a.Date),
                Vehicles = a.Vehicles.Select(v => new VehicleModel
                {
                    Id = v.Id,
                    VehicleType = Enum.Parse<VehicleType>(v.VehicleType),
                    Model = v.Model,
                    RegistrationDate = DateTime.ParseExact(v.RegistrationDate, "dd/MM/yyyy", System.Globalization.CultureInfo.DefaultThreadCurrentCulture),
                    Plate = v.Plate,
                    AdhesionDate = DateTime.ParseExact(v.AdhesionDate, "dd/MM/yyyy", System.Globalization.CultureInfo.DefaultThreadCurrentCulture),
                }).ToList(),
            }).ToList();

            return View(accidents);
        }

        [HttpGet]
        public IActionResult AddAccident()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddAccident(AddAccidentModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Date > DateTime.Now || model.Date < DateTime.Now.AddYears(-100))
            {
                ModelState.AddModelError(nameof(model.Date), "Not a valid date");
                return View(model);
            }

            var vPlates = new List<string>();
            if (model.Vehicles.Contains(","))
                vPlates = model.Vehicles.Split(",").ToList();
            else
                vPlates.Add(model.Vehicles);

            var vehicles = new List<VehicleEntity>();
            foreach (var v in vPlates)
            {
                if (context.Vehicle.Any(ve => ve.Plate == v))
                    vehicles.Add(context.Vehicle.Where(ve => ve.Plate == v).First());
                else
                {
                    ModelState.AddModelError(nameof(model.Vehicles), $"{v} is not a valid plate");
                    return View(model);
                }
            }

            var accident = new AccidentEntity
            {
                Date = model.Date.Value.ToString("dd/MM/yyyy"),
                Description = model.Description,
                Location = model.Location,
                Vehicles = vehicles,
            };

            context.Accident.Add(accident);
            context.SaveChanges();

            return RedirectToAction("ListAccidents");
        }
    }
}
