using ASPNetCoreECommerceSample.Data;
using ASPNetCoreECommerceSample.Entities;
using ASPNetCoreECommerceSample.Models;
using ASPNetCoreECommerceSample.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASPNetCoreECommerceSample.Controllers
{
    public class BannersController : Controller
    {
        private readonly ECommerceContext _context;
        private readonly ILogger<BannersController> _logger;

        public BannersController(ECommerceContext context, ILogger<BannersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Index()
        {

            var bannersList = _context.Banners.ToList();

            var productsModelList = new List<BannerModel>();
            foreach (var banner in bannersList)
            {
                productsModelList.Add(new BannerModel
                {
                    Title = banner.Title,
                    Description1 = banner.Description1,
                    Description2 = banner.Description2,
                    ImagePaths = _context.BannerImages
                    .Where(b => b.BannerId == banner.Id)
                    .Select(bi => bi.ImagePath).ToList() ?? new List<string>(),
                    Id = banner.Id
                });
            }


            return View(productsModelList);
        }

        [HttpGet]
        public ActionResult AddBanner()
        {          
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBanner([Bind("Name, Description, LongDescription, Images, Quantity, NormalPrice, SelectedCategoryId")] BannerModel bannerModel)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    return View(bannerModel);
                //}

                // Create a list to store the file paths associated with the product
                List<string> imagePaths = new List<string>();

                // Handle the uploaded images
                if (bannerModel.Images != null && bannerModel.Images.Count > 0)
                {
                    foreach (var imageFile in bannerModel.Images)
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

                // Create a new Banner entity and set its properties
                var bannerEntity = new Banner
                {
                    Title = bannerModel.Title,
                    Description1 = bannerModel.Description1,
                    Description2 = bannerModel.Description2,
                };

                _context.Banners.Add(bannerEntity);
                await _context.SaveChangesAsync();

                // Associate the uploaded image file paths with the product
                if (imagePaths.Count > 0)
                {
                    foreach (var imagePath in imagePaths)
                    {
                        var imageEntity = new BannerImage
                        {
                            BannerId = bannerEntity.Id,
                            ImagePath = imagePath
                        };

                        _context.BannerImages.Add(imageEntity);
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

            var banner = _context.Banners.Include(b => b.BannerImages).FirstOrDefault(x => x.Id == id);

            if (banner == null)
            {
                return NotFound();// Return a 404 Not Found response if the product is not found.
            }          


            var bannerModel = new BannerModel
            {
                Title = banner.Title,
                Id = banner.Id,
                Description1 = banner.Description1,
                Description2 = banner.Description2,
                ImagePaths = banner.BannerImages.Select(c => c.ImagePath).ToList()
            };

            return View(bannerModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BannerModel bannerModel)
        {
            if (!ModelState.IsValid)
            {
                return View(bannerModel);

            }

            var bannerEntity = _context.Banners.Find(bannerModel.Id);

            if (bannerEntity == null)
            {
                return NotFound();
            }

            //Lets update it
            bannerEntity.Title = bannerModel.Title;
            bannerEntity.Description1 = bannerModel.Description1;
            bannerEntity.Description2 = bannerModel.Description2;
       

            _context.SaveChanges();

            return RedirectToAction("Index");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {

            var bannerEntity = _context.Banners.Find(id);


            if (bannerEntity == null)
            {
                return NotFound();
            }


            //Remove the banner entity from the database
            _context.Banners.Remove(bannerEntity);
            _context.SaveChanges();

            // return RedirectToAction("Index");
            return Json(Url.Action("Index", "Banners"));
        }



        [HttpGet]
        public ActionResult BannerDetails(int id)
        {

            var product = _context.Banners.Find(id);

            if (product == null)
            {
                return NotFound(); // Return a 404 Not Found response if the product is not found.
            }

            // Create a BannerModel to pass the product details to the view
            var bannerModel = new BannerModel
            {
                Id = product.Id,
                Title = product.Title,
                Description1 = product.Description1,
                Description2 = product.Description2
            };

            // You also need to retrieve the associated images and add them to the Images property
            // You can do this by querying the database for images associated with the product

            // Example: Retrieve image paths from the BannerImages table for the product
            var imagePaths = _context.BannerImages.Where(pi => pi.BannerId == id).Select(pi => pi.ImagePath).ToList();

            // Set the image paths in the BannerModel
            bannerModel.ImagePaths = imagePaths;

            return View(bannerModel);
        }

    }
}