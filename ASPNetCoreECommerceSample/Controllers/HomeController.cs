using ASPNetCoreECommerceSample.Models;
using ASPNetCoreECommerceSample.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASPNetCoreECommerceSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly IBannerService _bannerService;
        private readonly IProductImageService _productImageService;

        public HomeController(ILogger<HomeController> logger, IProductService productService, IBannerService bannerService, IProductImageService productImageService)
        {
            _logger = logger;
            _productService = productService;
            _bannerService = bannerService;
            _productImageService = productImageService;
        }

        public IActionResult Index()
        {
            var bestSellerProducts = _productService.GetBestSellers();
            var saleItemProducts = _productService.GetSaleItems();
            var newArrivalProducts = _productService.GetNewArrivals();



            //Map Best Seller products entity to its Model version

            var bestSellerProductsModelList = bestSellerProducts.Select(product => new ProductModel
            {
                Name = product.Name,
                Description = product.Description,
                Quantity = product.Quantity,
                DiscountPrice = product.DiscountPrice,
                NormalPrice = product.NormalPrice,
                Rating = product.Rating ?? 0,
                HasDiscount = product.DiscountPrice > 0,
                ImagePaths = _productImageService.GetAllProductimages().Where(p => p.ProductId
                == product.Id).Select(c => c.ImagePath).ToList(),
                Id = product.Id
            }).ToList();


            //Map Sale Item products entity to its Model version

            var saleItemProductsModelList = saleItemProducts.Select(product => new ProductModel
            {
                Name = product.Name,
                Description = product.Description,
                Quantity = product.Quantity,
                DiscountPrice = product.DiscountPrice,
                NormalPrice = product.NormalPrice,
                Rating = product.Rating ?? 0,
                HasDiscount = product.DiscountPrice > 0,
                ImagePaths = _productImageService.GetAllProductimages().Where(p => p.ProductId
                == product.Id).Select(c => c.ImagePath).ToList(),
                Id = product.Id
            }).ToList();

            //Map New Arrival products entity to its Model version
            var newArrivalProductsModelList = newArrivalProducts.Select(product => new ProductModel
            {
                Name = product.Name,
                Description = product.Description,
                Quantity = product.Quantity,
                DiscountPrice = product.DiscountPrice,
                NormalPrice = product.NormalPrice,
                Rating = product.Rating ?? 0,
                HasDiscount = product.DiscountPrice > 0,
                ImagePaths = _productImageService.GetAllProductimages().Where(p => p.ProductId
                == product.Id).Select(c => c.ImagePath).ToList(),
                Id = product.Id
            }).ToList();





            //Get banners
            var banners = _bannerService.GetAllBanners();

            var bannerModelsList = banners.Select(banner => new BannerModel
            {
                Description1 = banner.Description1,
                Description2 = banner.Description2,
                Title = banner.Title,
                ImagePaths = banner.BannerImages.Select(b => b.ImagePath).ToList(),
            }).ToList();


            var landingPageModel = new LandingPageModel
            {
                BestSellerProducts = bestSellerProductsModelList,
                NewArrivalProducts = newArrivalProductsModelList,
                SaleItemProducts = saleItemProductsModelList,
                Banners = bannerModelsList,
            };

            return View(landingPageModel);
        }

        public IActionResult QuickView(int id)
        {
            var product = _productService.GetProductById(id);

            var productModel = new ProductModel
            {
                Name = product.Name,
                Description = product.Description,
                Quantity = product.Quantity,
                DiscountPrice = product.DiscountPrice,
                NormalPrice = product.NormalPrice,
                Rating = product.Rating ?? 0,
                HasDiscount = product.DiscountPrice > 0,
                ImagePaths = _productImageService.GetAllProductimages().Where(p => p.ProductId
                == product.Id).Select(c => c.ImagePath).ToList(),
                Id = product.Id
            };

            return PartialView("QuickView", productModel);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}