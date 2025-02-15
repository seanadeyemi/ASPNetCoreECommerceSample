using ASPNetCoreECommerceSample.Binders;
using ASPNetCoreECommerceSample.Data;
using ASPNetCoreECommerceSample.Entities.Identity;
using ASPNetCoreECommerceSample.Handlers;
using ASPNetCoreECommerceSample.Helpers;
using ASPNetCoreECommerceSample.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(config =>
{
    config.ModelBinderProviders.Insert(0, new ShoppingCartModelBinderProvider(new HttpContextAccessor()));
});

var services = builder.Services;
services.AddAuthorization(options =>
{
    options.AddPolicy("Atleast21", policy => policy.Requirements.Add(new MinimumAgeRequirement(21)));
});
services.AddScoped<IProductService, ProductService>();
services.AddScoped<IBannerService, BannerService>();
services.AddScoped<IProductImageService, ProductImageService>();
services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();
services.AddScoped<IImageService, ImageService>();
services.AddHttpContextAccessor();
services.AddSession();
services.AddDistributedMemoryCache();



services.AddDbContext<ECommerceContext>(options =>
{
    options.ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.NavigationBaseIncludeIgnored));

    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


//services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false).AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();


services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ECommerceContext>();


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

    //sign in settings
    options.SignIn.RequireConfirmedEmail = false;

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

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    try
    {
        var context = service.GetRequiredService<ECommerceContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = service.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
