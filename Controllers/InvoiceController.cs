using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlowInsurance.Entity;
using SlowInsurance.Models.Invoice;
using SlowInsurance.Models.Vehicle;
using SlowInsurance.Repo;
using System.Text;
using System.Text.Json;

namespace SlowInsurance.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        private const double DEFAULT_VALUE = 129.99;
        private readonly InsuranceDbContext context;

        public InvoiceController(InsuranceDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult ListInvoices(string? id)
        {
            var vehicles = context.Users.Where(u => u.UserName == User.Identity.Name).Include(u => u.Vehicles).ThenInclude(v => v.Invoices).First().Vehicles.ToList();

            if (id != null)
            {
                vehicles = vehicles.Where(v => v.Plate == id).ToList();
                ViewBag.HasId = true;
                id = null;
            }
            else
            {
                ViewBag.HasId = false;
            }

            var iEntities = vehicles.SelectMany(v => v.Invoices);
            var invoices = iEntities.Select(i => new InvoiceModel
            {
                Vehicle = JsonSerializer.Serialize(
                                            vehicles.Where(v => v.Invoices.Contains(i))
                                            .Select(v => new VehicleModel
                                            {
                                                Model = v.Model,
                                                Plate = v.Plate,
                                                VehicleType = Enum.Parse<VehicleType>(v.VehicleType),
                                                AdhesionDate = DateTime.ParseExact(v.AdhesionDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                                                RegistrationDate = DateTime.ParseExact(v.RegistrationDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                                                Id = v.Id
                                            })
                                            .First()),
                ExpirationDate = i.ExpirationDate == string.Empty ? DateTime.MinValue : DateTime.ParseExact(i.ExpirationDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                IssuedDate = DateTime.ParseExact(i.IssuedDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                PaymentType = Enum.Parse<PaymentType>(i.PaymentType),
                Value = i.Value,
            }).OrderByDescending(i => i.ExpirationDate != DateTime.MinValue).ThenBy(i => i.ExpirationDate).ToList();

            return View(invoices);
        }

        [HttpGet]
        public IActionResult AddInvoiceWithVehicle()
        {
            if(TempData["Vehicle"] == null)
                return NotFound();

            var vehicle = TempData["Vehicle"].ToString();

            var invoiceStart = new AddInvoiceModel
            {
                Value = DEFAULT_VALUE,
                Vehicle = vehicle,
            };

            return View(invoiceStart);
        }

        [HttpPost]
        public IActionResult AddInvoiceWithVehicle(AddInvoiceModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = context.Users.Where(u => u.UserName == User.Identity.Name).Include(u => u.Vehicles).First();
            
            var invoice = new InvoiceEntity
            {
                ExpirationDate = DateTime.Now.AddYears(1).ToShortDateString(),
                IssuedDate = DateTime.Now.ToShortDateString(),
                PaymentType = model.PaymentType.ToString(),
                Value = DEFAULT_VALUE,
            };
            var vModel = JsonSerializer.Deserialize<AddVehicleModel>(model.Vehicle);
            if (vModel.RegistrationDate > DateTime.Now)
            {
                ModelState.AddModelError("", "Not a valid date");
                return View(model);
            }
            var vehicle = new VehicleEntity
            {
                Model = vModel.Model,
                Plate = vModel.Plate,
                AdhesionDate = DateTime.Now.ToShortDateString(),
                RegistrationDate = vModel.RegistrationDate.ToShortDateString(),
                VehicleType = vModel.VehicleType.ToString(),
            };

            if (context.Vehicle.Any(v => v.Plate == vehicle.Plate))
            {
                ModelState.AddModelError("", "Could not create invoice.");
                return View(model);
            }
            user.Vehicles.Add(vehicle);
            user.Vehicles.Where(v => v.Plate == vehicle.Plate).First().Invoices.Add(invoice);
            context.SaveChanges();

            return RedirectToAction("ListVehicles", "Vehicle");
        }

        [HttpGet]
        public IActionResult RenewInvoice(string id)
        {
            var vehicles = context.Users.Where(u => u.UserName == User.Identity.Name).Include(u => u.Vehicles).First().Vehicles.Where(v => v.Plate == id);
            if (!vehicles.Any())
                return NotFound();

            var invoiceStart = vehicles.Select(v => new RenewInvoiceModel
            {
                Model = v.Model,
                Plate = v.Plate,
                Value = DEFAULT_VALUE,
            }).First();

            return View(invoiceStart);
        }

        [HttpPost]
        public IActionResult RenewInvoice(RenewInvoiceModel modell)
        {
            if (!ModelState.IsValid)
                return View(modell);

            var vehicles = context.Users.Where(u => u.UserName == User.Identity.Name).Include(u => u.Vehicles).ThenInclude(v => v.Invoices).First().Vehicles.Where(v => v.Plate == modell.Plate);
            if (!vehicles.Any())
                return NotFound();

            var invoice = vehicles.First().Invoices.OrderByDescending(i => i.ExpirationDate).First();
            if (DateTime.Parse(invoice.ExpirationDate).AddMonths(-1) > DateTime.Now)
            {
                ModelState.AddModelError("", "Does not need renewal");
                return View(modell);
            }

            var renewal = new InvoiceEntity
            {
                Value = DEFAULT_VALUE,
                ExpirationDate = DateTime.Now.AddYears(1).ToShortDateString(),
                IssuedDate = DateTime.Now.ToShortDateString(),
                PaymentType = modell.PaymentType.ToString(),
            };

            context.Users.Where(u => u.UserName == User.Identity.Name)
                .Include(u => u.Vehicles)
                .ThenInclude(v => v.Invoices)
                .First()
                .Vehicles
                .Where(v => v.Plate == modell.Plate)
                .First()
                .Invoices
                .Add(renewal);
            invoice.ExpirationDate = null;
            context.SaveChanges();

            return RedirectToAction("ListInvoices", "Invoice");
        }

    }
}
