 
using Microsoft.AspNetCore.Identity;
using System;
using System.Data;

namespace WebApplication1.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            //Seed Roles
            var userManager = service.GetService<UserManager<ApplicationUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();

            //Step1:Create all roles for the application
            //On first run off application Seed/Create roles.This is done on first run of the application

            await roleManager.CreateAsync(new IdentityRole(AppRoles.Role_GeneralUser.ToString()));
            await roleManager.CreateAsync(new IdentityRole(AppRoles.Role_Company.ToString()));
            await roleManager.CreateAsync(new IdentityRole(AppRoles.Role_Employee.ToString()));
            await roleManager.CreateAsync(new IdentityRole(AppRoles.Role_Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(AppRoles.Role_SuperAdmin.ToString()));

            //Step2:Create all roles for the application
            //After_Roles are created in the datbase,then create an Admin_Account.This will be the first Seeded account in the database.



            var user = new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                Name = "Admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            var userInDb = await userManager.FindByEmailAsync(user.Email);
            if (userInDb == null)
            {
                await userManager.CreateAsync(user, "P@ssword1");
                await userManager.AddToRoleAsync(user, AppRoles.Role_Admin.ToString());
            }
        }

    }
}
