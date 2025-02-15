using ASPNetCoreECommerceSample.Data;
using ASPNetCoreECommerceSample.Entities;
using ASPNetCoreECommerceSample.Models;
using ASPNetCoreECommerceSample.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASPNetCoreECommerceSample.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ECommerceContext _context;
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductImageService _productImageService;
		public ProductsController(ECommerceContext context, ILogger<ProductsController> logger, IProductImageService productImageService)
		{
			_context = context;
			_logger = logger;
			_productImageService = productImageService;
		}

		[HttpGet]
        public ActionResult Index()
        {

            var productsList = _context.Products.ToList();

            var productsModelList = new List<ProductModel>();
            foreach (var product in productsList)
            {
                productsModelList.Add(new ProductModel
                {
                    Name = product.Name,
                    //Color = product.Color,
                    LongDescription = product.LongDescription,
                    Description = product.Description,
                    Quantity = product.Quantity,
                    NormalPrice = product.NormalPrice,
                    Id = product.Id
                });
            }


            return View(productsModelList);
        }

        [HttpGet]
        public ActionResult AddProduct()
        {
            var categories = _context.Categories.ToList(); // Retrieve the list of categories from the database
            var productModel = new ProductModel
            {


                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
            };

            return View(productModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct([Bind("Name, Description, LongDescription, Images, Quantity, NormalPrice, SelectedCategoryId")] ProductModel productModel)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    return View(productModel);
                //}

                // Create a list to store the file paths associated with the product
                List<string> imagePaths = new List<string>();

                // Handle the uploaded images
                if (productModel.Images != null && productModel.Images.Count > 0)
                {
                    foreach (var imageFile in productModel.Images)
                    {
                        if (imageFile != null && imageFile.Length > 0)
                        {
                            // Save the image to a location of your choice, e.g., a folder on the server
                            // You can generate a unique file name to avoid overwriting existing images
                            var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await imageFile.CopyToAsync(stream);
                            }

                            // Store the file path or other information in your database for reference
                            // You can associate the file with the product being added
                            // Store the file path in the list
                            imagePaths.Add("images/" + fileName);
                        }
                    }
                }

                // Create a new Product entity and set its properties
                var productEntity = new Product
                {
                    Name = productModel.Name,
                    Quantity = productModel.Quantity,
                    Description = productModel.Description,
                    NormalPrice = productModel.NormalPrice,
                    LongDescription = productModel.LongDescription,
                    //Color = productModel.Color,
                    Category = await _context.Categories.FindAsync(productModel.SelectedCategoryId) // Set the Category property based on the selected category ID
                };

                _context.Products.Add(productEntity);
                await _context.SaveChangesAsync();

                // Associate the uploaded image file paths with the product
                if (imagePaths.Count > 0)
                {
                    foreach (var imagePath in imagePaths)
                    {
                        var imageEntity = new ProductImage
                        {
                            ProductId = productEntity.Id,
                            ImagePath = imagePath
                        };

                        _context.ProductImages.Add(imageEntity);
                    }

                    await _context.SaveChangesAsync();
                }

				// Redirect or navigate to the index page
				// return RedirectToAction("Index");
				return RedirectToAction(nameof(Index));
			}
            catch (Exception ex)
            {
                _logger.LogError("An error occured: {message}", ex.Message);
                throw;
            }
            
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {

            var product = _context.Products.Find(id);

            if (product == null)
            {
                return NotFound();// Return a 404 Not Found response if the product is not found.

            }




            var categories = _context.Categories.ToList(); // Retrieve the list of categories from the database




            var productModel = new ProductModel
            {
                Name = product.Name,
                Id = product.Id,
                Quantity = product.Quantity,
                //Color = product.Color,
                Description = product.Description,
                LongDescription = product.LongDescription,
                NormalPrice = product.NormalPrice,
                SelectedCategoryId = product.Category == null ? 0 : product.Category.Id, // Set the selected category ID
				ImagePaths = _productImageService.GetAllProductimages().Where(p => p.ProductId
== product.Id).Select(c => c.ImagePath).ToList(),
				Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
            };

            return View(productModel);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductModel productModel)
        {
            if (!ModelState.IsValid)
            {
                return View(productModel);

            }

            var productEntity = _context.Products.Find(productModel.Id);

            if (productEntity == null)
            {
                return NotFound();
            }

            var categoryEntity = _context.Categories.Find(productModel.SelectedCategoryId);

            if (categoryEntity == null)
            {
                // Add a custom validation error to ModelState
                ModelState.AddModelError("SelectedCategoryId", "The selected category is not valid. Please select a valid category.");
                return View(productModel);
            }


            //Lets update it
            productEntity.Name = productModel.Name;
            productEntity.Quantity = productModel.Quantity;
            //productEntity.Color = productModel.Color;
            productEntity.Description = productModel.Description;
            productEntity.NormalPrice = productModel.NormalPrice;
            productEntity.Category = categoryEntity;

            _context.SaveChanges();

            return RedirectToAction("Index");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {

            var productEntity = _context.Products.Find(id);


            if (productEntity == null)
            {
                return NotFound();
            }


            //Remove the product entity from the database
            _context.Products.Remove(productEntity);
            _context.SaveChanges();

            // return RedirectToAction("Index");
            return Json(Url.Action("Index", "Products"));

        }



        [HttpGet]
        public ActionResult ProductDetails(int id)
        {

            var product = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound(); // Return a 404 Not Found response if the product is not found.
            }

            // Create a ProductModel to pass the product details to the view
            var productModel = new ProductModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                //Color = product.Color,
                Quantity = product.Quantity,
                NormalPrice = product.NormalPrice,
                SelectedCategoryId = product.Category.Id // Set the selected category ID
            };

            // You also need to retrieve the associated images and add them to the Images property
            // You can do this by querying the database for images associated with the product

            // Example: Retrieve image paths from the ProductImages table for the product
            var imagePaths = _context.ProductImages.Where(pi => pi.ProductId == id).Select(pi => pi.ImagePath).ToList();

            // Set the image paths in the ProductModel
            productModel.ImagePaths = imagePaths;

            return View(productModel);
        }

    }
}