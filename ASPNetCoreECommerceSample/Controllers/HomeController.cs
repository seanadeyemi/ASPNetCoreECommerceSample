using ASPNetCoreECommerceSample.Entities;
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

        [HttpGet]
        public IActionResult ProductDetails(int id)
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

            // var reviews = new List<ReviewViewModel>
            // {
            //     new ReviewViewModel
            //     {
            //         UserName = "White Lewis",
            //         Rating = 5,
            //         Comment = "Great product!",
            //         Replies = new List<ReviewViewModel>
            //         {
            //             new ReviewViewModel
            //             {
            //                 UserName = "John Doe",
            //                 Rating = 4,
            //                 Comment = "I agree, very good product.",
            //                 Replies = new List<ReviewViewModel>()
            //             }
            //         }
            //     },
            //     new ReviewViewModel
            //     {
            //         UserName = "Alice Smith",
            //         Rating = 4,
            //         Comment = "Good value for money.",
            //         Replies = new List<ReviewViewModel>()
            //     }

            //};



            var productDetails = new ProductDetailsModel
            {
                Product = productModel,
                Reviews = product.Reviews
            .Where(r => r.ParentReviewId == null) // Only top-level reviews
            .Select(r => new ReviewViewModel
            {
                ReviewId = r.ReviewId,
                UserName = r.UserName,
                Rating = r.Rating,
                Comment = r.Comment,
                ProductId = r.ProductId,
                ParentReviewId = r.ParentReviewId,
                Replies = r.Replies.Select(reply => new ReviewViewModel
                {
                    ReviewId = reply.ReviewId,
                    UserName = reply.UserName,
                    Rating = reply.Rating,
                    Comment = reply.Comment,
                    ProductId = reply.ProductId,
                    ParentReviewId = reply.ParentReviewId,
                    Replies = reply.Replies.Select(secondReply => new ReviewViewModel
                    {
                        ReviewId = secondReply.ReviewId,
                        UserName = secondReply.UserName,
                        Rating = secondReply.Rating,
                        Comment = secondReply.Comment,
                        ProductId = secondReply.ProductId,
                        ParentReviewId = secondReply.ParentReviewId
                    }).ToList(),
                }).ToList()
            }).ToList()
                ,
                RelatedProducts = product.RelatedProducts.Select(rp =>
                {


                    return new RelatedProductViewModel
                    {
                        Id = rp.RelatedProduct.Id,
                        Name = rp.RelatedProduct.Name,
                        NormalPrice = rp.RelatedProduct.NormalPrice,
                        DiscountPrice = rp.RelatedProduct.DiscountPrice,
                        HasDiscount = rp.RelatedProduct.DiscountPrice > 0,
                        Rating = rp.RelatedProduct.Rating ?? 0,
                        Description = rp.RelatedProduct.Description,
                        ImagePaths = _productImageService.GetAllProductimages().Where(p => p.ProductId == rp.RelatedProduct.Id).Select(c => c.ImagePath).ToList()
                    };
                }).ToList()

            };

            return View(productDetails);
        }

        private ReviewViewModel MapToReviewViewModel(Review review)
        {
            return new ReviewViewModel
            {
                ReviewId = review.ReviewId,
                UserName = review.UserName,
                Rating = review.Rating,
                Comment = review.Comment,
                ParentReviewId = review.ParentReviewId,
                ProductId = review.ProductId,
                Replies = review.Replies.Select(r => MapToReviewViewModel(r)).ToList()
            };
        }


        [HttpPost]
        public IActionResult AddReview(int productId, ReviewViewModel reviewViewModel)
        {
            try
            {
                var product = _productService.GetProductById(productId);
                // Map ReviewViewModel to Review entity
                var review = new Review
                {
                    Email = reviewViewModel.Email,
                    UserName = reviewViewModel.UserName,
                    Rating = reviewViewModel.Rating,
                    Comment = reviewViewModel.Comment,
                    ProductId = productId,
                    ParentReviewId = reviewViewModel.ParentReviewId // Assuming ParentReviewId is passed in ReviewViewModel
                };

                // Add the new review to the product's reviews
                product.Reviews.Add(review);

                _productService.Save();

            }
            catch (Exception ex)
            {

                throw;
            }


            return RedirectToAction("ProductDetails", new { id = productId });
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