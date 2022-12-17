using EmailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlowInsurance.Entity;
using SlowInsurance.Models.Account;
using SlowInsurance.Repo;

namespace SlowInsurance.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ClientEntity> userManager;
        private readonly SignInManager<ClientEntity> signInManager;
        private readonly InsuranceDbContext context;
        private readonly IEmailSender emailSender;

        public AccountController(UserManager<ClientEntity> userManager, SignInManager<ClientEntity> signInManager, InsuranceDbContext context, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
            this.emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            if (signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(RegisterModel model)
        {
            if (signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");
            if (DateTime.Parse(model.Birthday) > DateTime.Now.AddYears(-18) && DateTime.Parse(model.Birthday) < DateTime.Now.AddYears(-118))
            {
                ModelState.AddModelError(nameof(model.Birthday), "Not a valid date");
                return View(model);
            }
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

        [HttpGet]
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
        public async Task<IActionResult> Login(LoginModel model)
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
            var model = new AccountDetailsModel
            {
                Email = user.UserName,
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
        public async Task<IActionResult> AccountDetails(AccountDetailsModel model)
        {
            if (DateTime.Parse(model.Birthday) > DateTime.Now.AddYears(-18) && DateTime.Parse(model.Birthday) < DateTime.Now.AddYears(-118))
            {
                ModelState.AddModelError("", "Not a valid date");
                return View(model);
            }
            if (ModelState.IsValid)
            {

                var user = await userManager.FindByNameAsync(model.Email);

                user.Email = model.EmailChanged;
                user.Name = model.Name;
                user.Address = model.Address;
                user.Birthday = model.Birthday;
                user.DriverLicense = model.DriverLicense;
                user.Historic = model.Historic;
                user.IBAN = model.IBAN;
                user.NIF = model.NIF;
                user.PhoneNumber = model.PhoneNumber;
                user.UserName = model.EmailChanged;

                var result = await userManager.UpdateAsync(user);

                if(!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.TryAddModelError(error.Code, error.Description);
                    }
                    return View(model);
                }
                
                await signInManager.RefreshSignInAsync(user);
                return View(new AccountDetailsModel
                {
                    Address = user.Address,
                    Birthday = user.Birthday,
                    DriverLicense = user.DriverLicense,
                    Email = user.Email,
                    EmailChanged = user.Email,
                    Historic = user.Historic,
                    IBAN = user.IBAN,
                    Name = user.Name,
                    NIF = user.NIF,
                    PhoneNumber = user.PhoneNumber,
                });
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return RedirectToAction("ForgotPasswordConfirmation");

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var callback = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);

            var message = new Message(new string[] { user.Email }, "Reset password token", callback);
            await emailSender.SendEmailAsync(message);

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
                return View(resetPasswordModel);

            var user = await userManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user == null)
                RedirectToAction(nameof(ResetPasswordConfirmation));

            var resetPassResult = await userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View();
            }
            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult DeleteAccount()
        {
            var user = context.Users.Where(u => u.UserName == User.Identity.Name).Include(u => u.Vehicles).First();
            if (user.Vehicles.Any())
            {
                TempData["Action"] = "Could not delete account. Contact our staff for details.";
                return RedirectToAction("AccountDetails");
            }

            signInManager.SignOutAsync();
            userManager.DeleteAsync(user);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public IActionResult ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("AccountDetails");

            var user = context.Users.First(u => u.UserName == User.Identity.Name);
            var result = userManager.ChangePasswordAsync(user, model.CurrentPassword, model.Password).Result;

            if (!result.Succeeded)
            {
                return RedirectToAction("AccountDetails");
            }

            return RedirectToAction("AccountDetails");
        }
    }
}
