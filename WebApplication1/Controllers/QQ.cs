 







using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class QQ : Controller
    {
        //Code to inject UserManager and IdentityManager
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IPasswordHasher<ApplicationUser> _passwordHasher;

        public QQ(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            //Code to inject UserManager and IdentityManager
            _roleManager = roleManager;
            _userManager = userManager;
            this._passwordHasher = passwordHasher;
        }




        public IActionResult Index()
        {
            var users = _userManager.Users;
            return View(users);
        }


        public IActionResult ChangePassword(string userId)
        {
            var model = new ChangePasswordViewModel234
            {
                UserId = userId
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    ModelState.AddModelError("", "Error updating password.");
            }

            return View();
        }



       







    }
}
