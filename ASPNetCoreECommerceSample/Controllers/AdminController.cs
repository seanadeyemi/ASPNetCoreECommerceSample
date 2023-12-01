using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASPNetCoreECommerceSample.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class AdminController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            //ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "Atleast21")]
        public IActionResult SpecialPage()
        {
            return View();
        }
    }
}
