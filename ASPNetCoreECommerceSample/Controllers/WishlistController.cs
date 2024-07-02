using ASPNetCoreECommerceSample.Data;
using ASPNetCoreECommerceSample.Entities;
using ASPNetCoreECommerceSample.Entities.Identity;
using ASPNetCoreECommerceSample.Models;
using ASPNetCoreECommerceSample.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNetCoreECommerceSample.Controllers
{
    public class WishlistController : Controller
    {
        private readonly ECommerceContext _context;
        private readonly IProductService _productService;
        private readonly IProductImageService _productImageService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public WishlistController(ECommerceContext context, IProductService productService, IProductImageService productImageService, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _productService = productService;
            _productImageService = productImageService;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index(string returnUrl)
        {

            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { returnUrl });
            }
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return Unauthorized(); // Return 401 status code for AJAX to handle
            //}


            // Retrieve the logged-in user's ID
            var customerId = GetCustomerId();
            var wishlist = await _context.Wishlists
                .Include(sc => sc.WishlistItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(sc => sc.CustomerId == customerId);

            var model = new WishlistModel
            {
                CustomerId = customerId,
                WishlistItems = wishlist?.WishlistItems.Select(i => new WishlistItemModel
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.Product.NormalPrice
                }).ToList() ?? new List<WishlistItemModel>()
            };
            model.ReturnUrl = returnUrl;

            return View(model);
        }

        public async Task<IActionResult> AddToWishlist(int productId, string returnUrl)
        {

            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(); // Return 401 status code for AJAX to handle
            }
            var product = _productService.GetProductById(productId);


            var customerId = GetCustomerId();
            var wishlists = await _context.Wishlists
                .Include(sc => sc.WishlistItems)
                .FirstOrDefaultAsync(sc => sc.CustomerId == customerId);

            if (wishlists == null)
            {
                wishlists = new Wishlist { CustomerId = customerId, WishlistItems = new List<WishlistItem>() };
                _context.Wishlists.Add(wishlists);
            }

            var wishlistItem = wishlists.WishlistItems.FirstOrDefault(i => i.ProductId == productId);
            if (wishlistItem == null)
            {
                wishlistItem = new WishlistItem { ProductId = productId, Quantity = 1, Wishlist = wishlists };
                wishlists.WishlistItems.Add(wishlistItem);

            }
            else
            {
                wishlistItem.Quantity += 1;
            }

            await _context.SaveChangesAsync();





            return RedirectToAction("Index", new { returnUrl });
        }

        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var customerId = GetCustomerId();
            var wishlists = await _context.Wishlists
                .Include(sc => sc.WishlistItems)
                .FirstOrDefaultAsync(sc => sc.CustomerId == customerId);

            if (wishlists != null)
            {
                var cartItem = wishlists.WishlistItems.FirstOrDefault(i => i.ProductId == productId);
                if (cartItem != null)
                {
                    wishlists.WishlistItems.Remove(cartItem);
                    await _context.SaveChangesAsync();
                }
            }


            return RedirectToAction("Index");
        }

        private string GetCustomerId()
        {
            return _userManager.GetUserId(User);
        }
        public PartialViewResult Summary(WishlistModel cart)
        {
            return PartialView(cart);
        }

        [HttpGet]
        public async Task<IActionResult> GetThumbnail(int productId, int width, int height)
        {
            try
            {

                var serverPath = _webHostEnvironment.WebRootPath;
                var thumbnailBytes = await _productImageService.GetProductThumbnailImage(serverPath, productId, width, height);

                return File(thumbnailBytes, "image/jpeg");
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        [HttpGet]
        public IActionResult GetCartItemCount(WishlistModel wishlist)
        {
            int itemCount = wishlist.WishlistItems.Sum(x => x.Quantity);
            return Json(itemCount);
        }

        [HttpGet]
        public IActionResult CartSummary(WishlistModel cart)
        {


            return PartialView("~/Views/Shared/_CartSummary.cshtml", cart);
        }
    }

}
