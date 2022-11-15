using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SlowInsurance.Entity;
using SlowInsurance.Models;
using SlowInsurance.Repo;

namespace SlowInsurance.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ClientEntity> userManager;
        private readonly SignInManager<ClientEntity> signInManager;
        private readonly InsuranceDbContext context;

        public AccountController(UserManager<ClientEntity> userManager, SignInManager<ClientEntity> signInManager, InsuranceDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            if (signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(RegisterViewModel model)
        {
            if (signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");
            if (ModelState.IsValid)
            {
                var user = new ClientEntity
                {
                    UserName = model.Email,
                    Name = model.Name,
                    Address = model.Address,
                    NIF = model.NIF,
                    IBAN = model.IBAN,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    Birthday = model.Birthday,
                    Historic = model.Historic,
                    DriverLicense = model.DriverLicense
                };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid Login Attempt");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult AccountDetails()
        {
            var user = context.Users.First(u => u.UserName == User.Identity.Name);
            var model = new AccountDetailsViewModel
            {
                Email = user.Email,
                EmailChanged = user.Email,
                Name = user.Name,
                Address = user.Address,
                Birthday = user.Birthday,
                DriverLicense = user.DriverLicense,
                Historic = user.Historic,
                IBAN = user.IBAN,
                NIF = user.NIF,
                PhoneNumber = user.PhoneNumber,
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AccountDetails(AccountDetailsViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = context.Users.First(u => u.UserName == model.Email);

                user.Email = model.EmailChanged;
                user.Name = model.Name;
                user.Address = model.Address;
                user.Birthday = model.Birthday;
                user.DriverLicense = model.DriverLicense;
                user.Historic = model.Historic;
                user.IBAN = model.IBAN;
                user.NIF = model.NIF;
                user.PhoneNumber = model.PhoneNumber;
                user.UserName = model.Email;

                await userManager.UpdateAsync(user);
            }
            return View(model);
        }
    }
}
