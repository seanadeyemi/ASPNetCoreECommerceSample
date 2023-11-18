using ASPNetCoreECommerceSample.Data;
using ASPNetCoreECommerceSample.Entities;
using ASPNetCoreECommerceSample.Models;

namespace ASPNetCoreECommerceSample.Services
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
    }

    public class ProductService : IProductService
    {
        private readonly ECommerceContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductService(ECommerceContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public List<Product> GetAllProducts()
        {
            var productsList = _context.Products
                    .OrderBy(p => p.Id)
                    .ToList();

            return productsList;
        }

        public void AddProduct(ProductModel productModel)
        {

            // Create a list to store the file paths associated with the product
            List<string> imagePaths = new List<string>();


            // Handle the uploaded images
            //if (productModel.Images != null && productModel.Images.Count > 0)
            //{
            //    foreach (var imageFile in productModel.Images)
            //    {
            //        if (imageFile != null && imageFile.ContentLength > 0)
            //        {
            //            // Save the image to a location of your choice, e.g., a folder on the server
            //            // You can generate a unique file name to avoid overwriting existing images
            //            var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            //            var filePath = Path.Combine(_environment.WebRootPath + "/Images", fileName);
            //            imageFile.SaveAs(filePath);

            //            // Store the file path or other information in your database for reference
            //            // You can associate the file with the product being added
            //            // Store the file path in the list
            //            imagePaths.Add("Images/" + fileName);
            //        }
            //    }
            //}



            //Save product to database

            // Create a new Product entity and set its properties
            var productEntity = new Product
            {
                Name = productModel.Name,
                Quantity = productModel.Quantity,
                Description = productModel.Description,
                NormalPrice = productModel.NormalPrice,
                //Color = productModel.Color,
                //Category = context.Categories.Find(productModel.SelectedCategoryId) // Set the Category property based on the selected category ID
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

    }
}
