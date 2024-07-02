using ASPNetCoreECommerceSample.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASPNetCoreECommerceSample.ViewComponents
{
    public class ShoppingCartSummaryViewComponent : ViewComponent
    {
        public ShoppingCartSummaryViewComponent()
        {

        }
        public IViewComponentResult Invoke(ShoppingCartModel cart)
        {

            return View("Summary.cshtml", cart);
        }
    }
}
