using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApplication1.Controllers;
using WebApplication1.Data;
 

var builder = WebApplication.CreateBuilder(args);

//AddedCode-------Add DatabaseServices to the container.------------AddedCode//
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


//AddedCode-------Identity*
//builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//            .AddDefaultUI()
//            .AddDefaultTokenProviders();


builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
{

 

    //previous code removed for clarity reasons
    opt.Lockout.AllowedForNewUsers = true;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
    opt.Lockout.MaxFailedAccessAttempts = 3;
}).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultUI()
            .AddDefaultTokenProviders();


builder.Services.AddScoped<ResetPasswordController>();
/*services.Configure<IdentityOptions>(opts =>
         {
             opts.SignIn.RequireConfirmedEmail = true;
         });*/


//AddedCode-------Add services to the container.------------AddedCode//

// Add application services.




//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
 

//builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

 
app.MapRazorPages();

//---------------------------------Seeder------------------------------------//
//
//On first run off application
//Seed/Create all roles for the application
//After_Roles are created in thedatbase,then create an Admin_Account.This will be the first Seeded account in the database.

using (var scope = app.Services.CreateScope())
{
    await DbSeeder.SeedRolesAndAdminAsync(scope.ServiceProvider);
}
//---------------------------------Seeder------------------------------------//

app.Run();
