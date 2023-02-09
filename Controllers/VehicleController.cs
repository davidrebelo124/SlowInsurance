using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SlowInsurance.Entity;
using SlowInsurance.Models;
using SlowInsurance.Models.File;
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
        private readonly IOptions<JsonOptions> options;

        public VehicleController(InsuranceDbContext context, IOptions<JsonOptions> options)
        {
            this.context = context;
            this.options = options;
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
                RegistrationDate = DateTime.ParseExact(v.RegistrationDate, "dd/MM/yyyy", System.Globalization.CultureInfo.DefaultThreadCurrentCulture),
                Plate = v.Plate,
                AdhesionDate = DateTime.ParseExact(v.AdhesionDate, "dd/MM/yyyy", System.Globalization.CultureInfo.DefaultThreadCurrentCulture),
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

            if (modell.RegistrationDate > DateTime.Now || modell.RegistrationDate < DateTime.Now.AddYears(-100))
            {
                ModelState.AddModelError(nameof(modell.RegistrationDate), "Not a valid date");
                return View(modell);
            }

            if (modell.VehicleType == VehicleType.Unknwon)
            {
                ModelState.AddModelError(nameof(modell.VehicleType), "Not a valid Vehicle Type");
                return View(modell);
            }

            TempData["Vehicle"] = Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes(modell));
            return RedirectToAction("AddInvoiceWithVehicle", "Invoice");
        }

        [HttpGet]
        public IActionResult PrintAll()
        {

            var clientEntity = context.Users.Where(u => u.UserName == User.Identity.Name).Include(u => u.Vehicles).ThenInclude(v => v.Invoices).First();
            var vehicles = clientEntity.Vehicles.ToList();
            var fileModel = new FileModel
            {
                User = new FileUserModel { Email = clientEntity.Email, Name = clientEntity.Name },
                Vehicles = vehicles.Select(
                    v => new FileVehicleModel
                    {
                        Model = v.Model,
                        Plate = v.Plate,
                        Invoices = v.Invoices.Select(
                            i => new FileInvoiceModel
                            {
                                Value = i.Value,
                                PaymentType = i.PaymentType,
                                ExpirationDate = i.ExpirationDate,
                                IssuedDate = i.IssuedDate
                            }).ToList()
                    }).ToList(),
            };

            var json = JsonSerializer.Serialize(fileModel, options.Value.JsonSerializerOptions);
            var file = Encoding.Unicode.GetBytes(json);

            return File(file, "application/json", $"{User.Identity.Name}_Vehicles&Invoices.json");
        }

        [HttpGet]
        public IActionResult Print(string id)
        {

            var clientEntity = context.Users.Where(u => u.UserName == User.Identity.Name).Include(u => u.Vehicles).ThenInclude(v => v.Invoices).First();
            var v = clientEntity.Vehicles.Where(v => v.Plate == id).First();
            var fileModel = new FileModel
            {
                User = new FileUserModel { Email = clientEntity.Email, Name = clientEntity.Name },
                Vehicles = new List<FileVehicleModel>
                {
                    new FileVehicleModel
                    {
                        Model = v.Model,
                        Plate = v.Plate,
                        Invoices = v.Invoices.Select(
                            i => new FileInvoiceModel
                            {
                                Value = i.Value,
                                PaymentType = i.PaymentType,
                                ExpirationDate = i.ExpirationDate,
                                IssuedDate = i.IssuedDate
                            }).ToList()
                    },
                }
            };

            var json = JsonSerializer.Serialize(fileModel, options.Value.JsonSerializerOptions);
            var file = Encoding.Unicode.GetBytes(json);

            return File(file, "application/json", $"{User.Identity.Name}_{id}_Invoices.json");
        }
    }
}
