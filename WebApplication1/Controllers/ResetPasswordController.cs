using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

using Microsoft.EntityFrameworkCore;



namespace WebApplication1.Controllers
{
    public class ResetPasswordController : Controller
    {



        //Code to inject UserManager and IdentityManager
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IPasswordHasher<ApplicationUser> _passwordHasher;
        public ResetPasswordController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            //Code to inject UserManager and IdentityManager
            _roleManager = roleManager;
            _userManager = userManager;
            this._passwordHasher = passwordHasher;
        }

        //Code for Index Page
        public async Task<IActionResult> Index()
        {


            var users = await _userManager.Users.ToListAsync();
            // Set the users list in ViewBag
            ViewBag.Users = users;

            var userRolesViewModel = new List<UserRolesViewModel>();
            foreach (ApplicationUser user in users)
            {
                var thisViewModel = new UserRolesViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                //thisViewModel.FirstName = user.FirstName;
                //thisViewModel.LastName = user.LastName;
                thisViewModel.Name = user.Name;
                thisViewModel.Roles = await GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);
            }
            return View(userRolesViewModel);
        }


        //Code to Get User_Roles
        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]

        //public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    var user = await _userManager.FindByIdAsync(model.UserId);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    // Hash the new password provided by the admin
        //    var hashedPassword = _passwordHasher.HashPassword(user, model.NewPassword);

        //    // Update the user's password
        //    user.PasswordHash = hashedPassword;
        //    var result = await _userManager.UpdateAsync(user);
        //    if (!result.Succeeded)
        //    {
        //        // Handle password update error
        //        ModelState.AddModelError("", "Error updating password");
        //        return View(model);
        //    }

        //    // Password reset successful
        //    return RedirectToAction("Index", "UserRoles");
        //}




        //[HttpPost]
        //public async Task<IActionResult> Update(string id, string email, string password)
        //{
        //    var user = await _userManager.FindByIdAsync(id);
        //    if (user != null)
        //    {
        //        if (!string.IsNullOrEmpty(email))
        //            user.Email = email;
        //        else
        //            ModelState.AddModelError("", "Email cannot be empty");

        //        if (!string.IsNullOrEmpty(password))
        //            user.PasswordHash = _passwordHasher.HashPassword(user, password);
        //        else
        //            ModelState.AddModelError("", "Password cannot be empty");

        //        if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
        //        {
        //            IdentityResult result = await _userManager.UpdateAsync(user);
        //            if (result.Succeeded)
        //                return RedirectToAction("Index");
        //            else
        //                ModelState.AddModelError("", "Error updating password");
        //        }
        //    }
        //    else
        //        ModelState.AddModelError("", "User Not Found");
        //    return View(user);
        //}



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ResetPasswordT(string userId)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    // Generate a temporary password
        //    string temporaryPassword = GenerateTemporaryPassword();

        //    // Update the user's password in the database
        //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //    var result = await _userManager.ResetPasswordAsync(user, token, temporaryPassword);
        //    if (!result.Succeeded)
        //    {
        //        // Handle password reset error
        //        ModelState.AddModelError("", "Error resetting password");
        //        return View();
        //    }

        //    // Provide the temporary password to the user through a secure channel (e.g., display it on a webpage or send it via SMS)
        //    TempData["TemporaryPassword"] = temporaryPassword;

        //    return RedirectToAction("Index");
        //}



        //private string GenerateTemporaryPassword()
        //{
        //    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        //    var random = new Random();
        //    var passwordLength = 8; // Adjust the length as per your requirements

        //    var password = new string(Enumerable.Repeat(chars, passwordLength)
        //        .Select(s => s[random.Next(s.Length)]).ToArray());

        //    return password;
        //}












        //working model-password reset1
        [HttpGet]
        public IActionResult ResetPassword1()
        {
            return View();
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword1(PasswordResetViewModel1 model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                    return View(model);
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

                if (result.Succeeded)
                {
                    // Password reset successful, you can redirect to a success page or login page
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If model state is invalid, return the view with validation errors
            return View(model);
        }
    }







 






















}
