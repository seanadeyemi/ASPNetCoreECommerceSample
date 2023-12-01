using ASPNetCoreECommerceSample.Data;
using ASPNetCoreECommerceSample.Entities;
using ASPNetCoreECommerceSample.Models;
using System.ComponentModel.DataAnnotations;

namespace ASPNetCoreECommerceSample.Services
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        List<Product> GetNewArrivals();
        List<Product> GetBestSellers();
        List<Product> GetSaleItems();
        Product GetProductById(int id);
    }

    public class ProductService : IProductService
    {
        private readonly ECommerceContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductService(ECommerceContext _context, IWebHostEnvironment environment)
        {
            this._context = _context;
            _environment = environment;
        }

        public List<Product> GetAllProducts()
        {
            var productsList = _context.Products
                    .OrderBy(p => p.Id)
                    .ToList();

            return productsList;
        }



        public Product GetProductById(int id)
        {


            var product = _context.Products.Find(id);

            if (product == null)
            {
                throw new ValidationException("Id is required"); // Return a 404 Not Found response if the product is not found.

            }


            return product;
        }



        public async Task AddProduct(ProductModel productModel)
        {

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
                        var filePath = Path.Combine(_environment.WebRootPath + "/Images", fileName);
                        // imageFile.SaveAs(filePath);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        // Store the file path or other information in your database for reference
                        // You can associate the file with the product being added
                        // Store the file path in the list
                        imagePaths.Add("Images/" + fileName);
                    }
                }
            }



            //Save product to database

            var availableColors = productModel.AvailableColors.Select(colorName => _context.ProductColors.FirstOrDefault(c => c.Name == colorName)).ToList();

            var availableSizes = productModel.AvailableSizes.Select(sizeName => _context.ProductSizes.FirstOrDefault(c => c.Name == sizeName)).ToList();

            // Create a new Product entity and set its properties
            var productEntity = new Product
            {
                Name = productModel.Name,
                Quantity = productModel.Quantity,
                Description = productModel.Description,
                NormalPrice = productModel.NormalPrice,
                DiscountPrice = productModel.DiscountPrice,
                LongDescription = productModel.LongDescription,
                AvailableSizes = availableSizes,
                AvailableColors = availableColors,
                DateAdded = DateTime.UtcNow,
                Rating = productModel.Rating,

                // Set the ProductCategories property based on the selected category ID
                ProductCategories = new List<ProductCategory>
                {
                    new ProductCategory
                    {
                        CategoryId = productModel.SelectedCategoryId
                    }
                }
            };


            _context.Products.Add(productEntity);
            _context.SaveChanges();


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

                _context.SaveChanges();
            }

        }

        public List<Product> GetNewArrivals()
        {
            DateTime sevenDaysAgo = DateTime.Now.Date.AddDays(-7);

            var newArrivals = _context.Products
                .Where(p => p.DateAdded >= sevenDaysAgo)
                .OrderBy(p => p.DateAdded)
                .ToList();

            return newArrivals;
        }

        public List<Product> GetBestSellers()
        {
            var bestSellers = _context.Products
            .OrderByDescending(p => p.Quantity)
            .ToList();

            return bestSellers;
        }

        public List<Product> GetSaleItems()
        {
            var saleItems = _context.Products
          .Where(p => p.DiscountPrice > 0)
          .OrderBy(p => p.Id)
          .ToList();

            return saleItems;
        }

    }
}
