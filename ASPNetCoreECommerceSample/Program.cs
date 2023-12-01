using ASPNetCoreECommerceSample.Data;
using ASPNetCoreECommerceSample.Entities.Identity;
using ASPNetCoreECommerceSample.Handlers;
using ASPNetCoreECommerceSample.Helpers;
using ASPNetCoreECommerceSample.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var services = builder.Services;
services.AddAuthorization(options =>
{
    options.AddPolicy("Atleast21", policy => policy.Requirements.Add(new MinimumAgeRequirement(21)));
});
services.AddScoped<IProductService, ProductService>();
services.AddScoped<IBannerService, BannerService>();
services.AddScoped<IProductImageService, ProductImageService>();
services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();


services.AddDbContext<ECommerceContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false).AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();


services.AddAutoMapper(typeof(Program));

services.Configure<IdentityOptions>(options =>
{

    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;


});


services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    options.LoginPath = "/Account/LoginRegister";
    //options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
