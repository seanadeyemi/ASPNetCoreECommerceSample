using Microsoft.AspNetCore.Mvc;

namespace ASPNetCoreECommerceSample.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
