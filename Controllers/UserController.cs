using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SlowInsurance.Entity;
using SlowInsurance.Models;

namespace SlowInsurance.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ClientEntity> userManager;
        private readonly SignInManager<ClientEntity> signInManager;

        public UserController(UserManager<ClientEntity> userManager, SignInManager<ClientEntity> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(RegisterViewModel model)
        {
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
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
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
    }
}
