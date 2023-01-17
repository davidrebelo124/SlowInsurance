using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SlowInsurance.Entity;
using SlowInsurance.Models.Accident;
using SlowInsurance.Models.Admin;
using SlowInsurance.Models.Vehicle;
using SlowInsurance.Repo;

namespace SlowInsurance.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ClientEntity> userManager;
        private readonly InsuranceDbContext context;

        public AdminController(UserManager<ClientEntity> userManager, InsuranceDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }

        [HttpGet]
        public IActionResult MainPage() => View();

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = context.Users
                .AsEnumerable()
                .Select(
                    u => new ListUserModel
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Email= u.Email,
                        IsAdmin = userManager.IsInRoleAsync(u, "Admin").Result,
                    }
                ).ToList();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> GiveAdminRoleAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (await userManager.IsInRoleAsync(user, "Admin"))
                return RedirectToAction("ListUser");

            await userManager.AddToRoleAsync(user, "Admin");
            return RedirectToAction("ListUsers");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveAdminRoleAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (!await userManager.IsInRoleAsync(user, "Admin"))
                return RedirectToAction("ListUser");

            await userManager.RemoveFromRoleAsync(user, "Admin");
            return RedirectToAction("ListUsers");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            await userManager.DeleteAsync(await userManager.FindByIdAsync(id));
            return RedirectToAction("ListUsers");
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
