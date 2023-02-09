using EmailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PhoneNumbers;
using Portugal.Nif.Validator;
using SlowInsurance.Entity;
using SlowInsurance.Models.Account;
using SlowInsurance.Models.Invoice;
using SlowInsurance.Repo;
using System.Text;
using System.Text.Json;

namespace SlowInsurance.Controllers
{
    public class AccountController : Controller
    {
        private readonly IOptions<JsonOptions> options;
        private readonly UserManager<ClientEntity> userManager;
        private readonly SignInManager<ClientEntity> signInManager;
        private readonly InsuranceDbContext context;
        private readonly IEmailSender emailSender;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly INifValidator nifValidator;
        private readonly PhoneNumberUtil pUtil;

        public AccountController(
                        IOptions<JsonOptions> options,
                        UserManager<ClientEntity> userManager,
                        SignInManager<ClientEntity> signInManager,
                        InsuranceDbContext context,
                        IEmailSender emailSender,
                        RoleManager<IdentityRole> roleManager,
                        INifValidator nifValidator
            )
        {
            this.options = options;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
            this.emailSender = emailSender;
            this.roleManager = roleManager;
            this.nifValidator = nifValidator;
            pUtil = PhoneNumberUtil.GetInstance();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            if (signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "sHome");
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(CreateAccountModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userData = Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes(model));
            TempData["UserData"] = userData;

            return RedirectToAction("CompleteSignUp");
        }

        [HttpGet]
        public IActionResult CompleteSignUp()
        {
            if (TempData["UserData"] is null)
                return NotFound();

            var userData = JsonSerializer.Deserialize<CreateAccountModel>(TempData["UserData"]!.ToString()!);

            var continueSignUp = new CompleteRegisterModel
            {
                Email = userData!.Email,
                ConfirmPassword = userData.ConfirmPassword,
                Password = userData.Password!,
                Name = userData.Name,
            };

            return View(continueSignUp);
        }

        [HttpPost]
        public async Task<IActionResult> CompleteSignUp(CompleteRegisterModel model)
        {
            model.Password = model.ConfirmPassword;
            ModelState.Clear();
            TryValidateModel(model);
            
            if (!ModelState.IsValid)
                return View(model);
            
            if (signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");
            
            if (DateTime.Parse(model.Birthday!) > DateTime.Now.AddYears(-18) || DateTime.Parse(model.Birthday!) < DateTime.Now.AddYears(-118))
                ModelState.AddModelError(nameof(model.Birthday), "Not a valid date");
            // Validation not working
            //if (!pUtil.IsValidNumberForRegion(pUtil.Parse(model.PhoneNumber, "PT"), "351"))
            //    ModelState.AddModelError(nameof(model.PhoneNumber), "Not a valid phone number");
            
            if (!nifValidator.Validate(model.NIF))
                ModelState.AddModelError(nameof(model.NIF), "Not a valid NIF");

            if (ModelState.ErrorCount > 0)
                return View(model);

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

                //// Create Admin Role
                //var adminExists = await roleManager.FindByNameAsync("Admin");
                //if (adminExists is null)
                //{
                //    await roleManager.CreateAsync(new IdentityRole("Admin"));
                //    await userManager.AddToRoleAsync(userManager.FindByIdAsync("8da842fc-7aba-4cbb-9668-a7e56e92ad96").Result, "Admin");
                //}

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
            if (!ModelState.IsValid)
                return View(model);
            if (DateTime.Parse(model.Birthday!) > DateTime.Now.AddYears(-18) || DateTime.Parse(model.Birthday!) < DateTime.Now.AddYears(-118))
            {
                ModelState.AddModelError(nameof(model.Birthday), "Not a valid date");
                return View(model);
            }
            if (!pUtil.IsValidNumber(pUtil.Parse(model.PhoneNumber, "PT")))
            {
                ModelState.AddModelError(nameof(model.PhoneNumber), "Not a valid phone number");
                return View(model);
            }
            if (!nifValidator.Validate(model.NIF))
            {
                ModelState.AddModelError(nameof(model.NIF), "Not a valid NIF");
                return View(model);
            }
            if (ModelState.IsValid)
            {

                var user = await userManager.FindByNameAsync(model.Email);

                user.Email = model.EmailChanged;
                user.Name = model.Name;
                user.Address = model.Address;
                user.Birthday = model.Birthday;
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

            // email html
            var content = $"<a class=\"btn btn-primary btn-lg\" href=\"{callback}\">Click here to reset your password</a>";

            var message = new Message(new string[] { user.Email }, "Reset your password | Slow Insurance", content);
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
                return RedirectToAction("ResetPassword", new { token = resetPasswordModel.Token, email = resetPasswordModel.Email});

            var user = await userManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user is null)
                RedirectToAction(nameof(ResetPasswordConfirmation));

            var resetPassResult = await userManager.ResetPasswordAsync(user!, resetPasswordModel.Token, resetPasswordModel.Password);
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
            if (user.Vehicles.Any() || User.IsInRole("Admin"))
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
                return PartialView("_ChangePassword", model);

            var user = context.Users.First(u => u.UserName == User.Identity.Name);
            var result = userManager.ChangePasswordAsync(user, model.CurrentPassword, model.Password).Result;

            if (!result.Succeeded)
                return PartialView(model);

            return PartialView(model);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult PrintDetails()
        {
            var user = context.Users.First(u => u.UserName == User.Identity.Name);
            var model = new AccountDetailsModel
            {
                Email = user.UserName,
                Name = user.Name,
                Address = user.Address,
                Birthday = user.Birthday,
                Historic = user.Historic,
                IBAN = user.IBAN,
                NIF = user.NIF,
                PhoneNumber = user.PhoneNumber,
            };

            var json = JsonSerializer.Serialize(model, options.Value.JsonSerializerOptions);
            var file = Encoding.Unicode.GetBytes(json);
            return File(file, "application/json", $"{User.Identity.Name}_Details.json");
        }
    }
}
