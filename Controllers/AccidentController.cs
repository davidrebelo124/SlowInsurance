using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlowInsurance.Entity;
using SlowInsurance.Models.Accident;
using SlowInsurance.Models.Vehicle;
using SlowInsurance.Repo;
using System.Globalization;

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
            var vehicles = context.Users.Where(u => u.UserName == User.Identity.Name).Include(u => u.Vehicles).First().Vehicles.ToList();

            var accidents = context.Accident
            .Include(a => a.Vehicles)
            .AsEnumerable()
            .Where(a => vehicles.Intersect(a.Vehicles).Any())
            .Select(a => new AccidentModel
            {
                Location = a.Location,
                Description = a.Description,
                Date = DateTime.Parse(a.Date),
                Vehicles = a.Vehicles.Select(v => new VehicleModel
                {
                    Id = v.Id,
                    VehicleType = Enum.Parse<VehicleType>(v.VehicleType!),
                    Model = v.Model,
                    RegistrationDate = DateTime.TryParseExact(v.RegistrationDate, "dd/MM/yyyy", CultureInfo.DefaultThreadCurrentCulture, DateTimeStyles.None, out DateTime registrationDate) ? registrationDate : default,
                    Plate = v.Plate,
                    AdhesionDate = DateTime.TryParseExact(v.AdhesionDate, "dd/MM/yyyy", CultureInfo.DefaultThreadCurrentCulture, DateTimeStyles.None, out DateTime adhesionDate) ? adhesionDate : default,
                }).ToList(),
            }).ToList();

            return View(accidents);
        }

    }
}
