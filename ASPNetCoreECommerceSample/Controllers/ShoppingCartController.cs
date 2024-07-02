using ASPNetCoreECommerceSample.Binders;
using ASPNetCoreECommerceSample.Data;
using ASPNetCoreECommerceSample.Entities.Identity;
using ASPNetCoreECommerceSample.Extensions;
using ASPNetCoreECommerceSample.Models;
using ASPNetCoreECommerceSample.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASPNetCoreECommerceSample.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ECommerceContext _context;
        private readonly IProductService _productService;
        private readonly IProductImageService _productImageService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShoppingCartController(ECommerceContext context, IProductService productService, IProductImageService productImageService, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _productService = productService;
            _productImageService = productImageService;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index([ModelBinder(typeof(ShoppingCartModelBinder))] ShoppingCartModel cart, string returnUrl)
        {
            //var customerId = GetCustomerId();
            //var shoppingCart = await _context.ShoppingCarts
            //    .Include(sc => sc.Items)
            //    .ThenInclude(i => i.Product)
            //    .FirstOrDefaultAsync(sc => sc.CustomerId == customerId);

            //var model = new ShoppingCartModel
            //{
            //    CustomerId = customerId,
            //    Items = shoppingCart?.Items.Select(i => new ShoppingCartItemModel
            //    {
            //        ProductId = i.ProductId,
            //        ProductName = i.Product.Name,
            //        Quantity = i.Quantity,
            //        UnitPrice = i.Product.NormalPrice
            //    }).ToList() ?? new List<ShoppingCartItemModel>()
            //  };

            //return View(model);
            cart.ReturnUrl = returnUrl;

            return View(cart);
        }

        public async Task<IActionResult> AddToCart([ModelBinder(typeof(ShoppingCartModelBinder))] ShoppingCartModel cart, int productId, string returnUrl, bool IsPartial = true)
        {
            //var customerId = GetCustomerId();
            //var shoppingCart = await _context.ShoppingCarts
            //    .Include(sc => sc.Items)
            //    .FirstOrDefaultAsync(sc => sc.CustomerId == customerId);

            //if (shoppingCart == null)
            //{
            //    shoppingCart = new ShoppingCart { CustomerId = customerId, Items = new List<ShoppingCartItem>() };
            //    _context.ShoppingCarts.Add(shoppingCart);
            //}

            //var cartItem = shoppingCart.Items.FirstOrDefault(i => i.ProductId == productId);
            //if (cartItem == null)
            //{
            //    cartItem = new ShoppingCartItem { ProductId = productId, Quantity = quantity, ShoppingCart = shoppingCart };
            //    shoppingCart.Items.Add(cartItem);

            //}
            //else
            //{
            //    cartItem.Quantity += quantity;
            //}

            //await _context.SaveChangesAsync();

            var product = _productService.GetProductById(productId);


            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (cartItem == null)
            {
                var item = new ShoppingCartItemModel
                {
                    ProductId = productId,
                    ProductName = product.Name,
                    Quantity = 1,
                    ProductImagePaths = _productImageService.GetAllProductimages().Where(p => p.ProductId
                    == product.Id).Select(c => c.ImagePath).ToList(),
                    UnitPrice = product.DiscountPrice > 0 ? product.DiscountPrice
                              : product.NormalPrice
                };

                cart.AddItem(item);
            }
            else
            {
                cartItem.Quantity += 1;
            }


            // Save the cart to session
            HttpContext.Session.SetObjectAsJson("Cart", cart);

            if (IsPartial)
            {
                return RedirectToAction("Summary");
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public async Task<IActionResult> RemoveFromCart([ModelBinder(typeof(ShoppingCartModelBinder))] ShoppingCartModel cart, int productId)
        {
            //var customerId = GetCustomerId();
            //var shoppingCart = await _context.ShoppingCarts
            //    .Include(sc => sc.Items)
            //    .FirstOrDefaultAsync(sc => sc.CustomerId == customerId);

            //if (shoppingCart != null)
            //{
            //    var cartItem = shoppingCart.Items.FirstOrDefault(i => i.ProductId == productId);
            //    if (cartItem != null)
            //    {
            //        shoppingCart.Items.Remove(cartItem);
            //        await _context.SaveChangesAsync();
            //    }
            //}
            var item = cart.Items.Find(i => i.ProductId == productId);
            if (item != null)
            {
                cart.RemoveItem(item);

                // Save the cart to session
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }

        private int GetCustomerId()
        {
            // Replace with your logic to get the current customer ID
            return 1;
        }
        public PartialViewResult Summary(ShoppingCartModel cart)
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
        public IActionResult GetCartItemCount(ShoppingCartModel cart)
        {
            int itemCount = cart.Items.Sum(x => x.Quantity);
            return Json(itemCount);
        }

        [HttpGet]
        public IActionResult CartSummary([ModelBinder(BinderType = typeof(ShoppingCartModelBinder))] ShoppingCartModel cart)
        {
            //var cartSummaryViewModel = new CartSummaryViewModel
            //{
            //    // Populate the view model with the necessary data
            //};

            return PartialView("~/Views/Shared/_CartSummary.cshtml", cart);
        }
    }

}
