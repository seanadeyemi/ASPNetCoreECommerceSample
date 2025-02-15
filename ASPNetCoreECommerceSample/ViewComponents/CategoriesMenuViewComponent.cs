using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ASPNetCoreECommerceSample.Models;
using ASPNetCoreECommerceSample.Data;

namespace ASPNetCoreECommerceSample.ViewComponents
{
    public class CategoriesMenuViewComponent : ViewComponent
    {
        private readonly ECommerceContext _context;

        public CategoriesMenuViewComponent(ECommerceContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _context.Categories
                                           .Include(c => c.SubCategories)
                                           .Where(c => c.ParentCategoryId == null)
                                           .ToListAsync();
            return View(categories);
        }
    }
}
