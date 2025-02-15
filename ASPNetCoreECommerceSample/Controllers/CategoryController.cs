using ASPNetCoreECommerceSample.Data;
using ASPNetCoreECommerceSample.Entities;
using ASPNetCoreECommerceSample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASPNetCoreECommerceSample.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ECommerceContext _context;

        public CategoryController(ECommerceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categoryList = await _context.Categories.ToListAsync();

            var categoryModelList = categoryList.Select(category => new CategoryModel
            {
                Id = category.Id,
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId,
                ParentCategoryName = category.ParentCategory?.Name
            }).ToList();

            return View(categoryModelList);
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            ViewBag.ParentCategories = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory([Bind("Name, ParentCategoryId")] CategoryModel categoryModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ParentCategories = new SelectList(_context.Categories, "Id", "Name", categoryModel.ParentCategoryId);
                return View(categoryModel);
            }

            var categoryEntity = new Category
            {
                Name = categoryModel.Name,
                ParentCategoryId = categoryModel.ParentCategoryId
            };

            _context.Categories.Add(categoryEntity);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            var categoryModel = new CategoryModel
            {
                Id = category.Id,
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId
            };

            ViewBag.ParentCategories = new SelectList(_context.Categories, "Id", "Name", category.ParentCategoryId);

            return View(categoryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryModel categoryModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ParentCategories = new SelectList(_context.Categories, "Id", "Name", categoryModel.ParentCategoryId);
                return View(categoryModel);
            }

            var categoryEntity = await _context.Categories.FindAsync(categoryModel.Id);

            if (categoryEntity == null)
            {
                return NotFound();
            }

            categoryEntity.Name = categoryModel.Name;
            categoryEntity.ParentCategoryId = categoryModel.ParentCategoryId;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var categoryEntity = await _context.Categories.FindAsync(id);

            if (categoryEntity == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(categoryEntity);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult CategoriesMenu()
        {
            var categories = _context.Categories
                                     .Include(c => c.SubCategories)
                                     .Where(c => c.ParentCategoryId == null)
                                     .ToList();
            return PartialView("~/Views/Shared/_CategoriesMenu.cshtml", categories);
        }
    }

}