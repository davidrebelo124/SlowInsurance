using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SlowInsurance.Entity;
using SlowInsurance.Models.Admin;
using SlowInsurance.Repo;

namespace SlowInsurance.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ClientEntity> userManager;

        public AdminController(UserManager<ClientEntity> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult MainPage() => View();

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.GetUsersInRoleAsync("Admin").Result
                .Select(
                    u => new ListUserModel
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Email= u.Email,
                        IsAdmin = true,
                    }
                ).ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult ListUsers(string email)
        {
            var client = userManager.FindByEmailAsync(email).Result;
            var user = new ListUserModel
            {
                Id = client.Id,
                Name = client.Name,
                Email = client.Email,
                IsAdmin = userManager.IsInRoleAsync(client, "Admin").Result,
            };
            var userModel = new List<ListUserModel>
            {
                user
            };
            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> GiveAdminRoleAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (await userManager.IsInRoleAsync(user, "Admin"))
                return RedirectToAction("ListUser");

            await userManager.AddToRoleAsync(user, "Admin");
            return RedirectToAction("ListUsers");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveAdminRoleAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (!await userManager.IsInRoleAsync(user, "Admin"))
                return RedirectToAction("ListUser");

            await userManager.RemoveFromRoleAsync(user, "Admin");
            return RedirectToAction("ListUsers");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(string userId)
        {
            await userManager.DeleteAsync(await userManager.FindByIdAsync(userId));
            return RedirectToAction("ListUsers");
        }

    }
}
