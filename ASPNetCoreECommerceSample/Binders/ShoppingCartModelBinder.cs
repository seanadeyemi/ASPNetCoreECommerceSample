using ASPNetCoreECommerceSample.Extensions;
using ASPNetCoreECommerceSample.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ASPNetCoreECommerceSample.Binders
{
    public class ShoppingCartModelBinder : IModelBinder
    {
        private const string key = "Cart";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShoppingCartModelBinder(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var httpContext = _httpContextAccessor.HttpContext;
            ShoppingCartModel cart = null;

            if (httpContext.Session != null)
            {
                cart = httpContext.Session.GetObjectFromJson<ShoppingCartModel>(key);
            }

            if (cart == null)
            {
                cart = new ShoppingCartModel();
                if (httpContext.Session != null)
                {
                    httpContext.Session.SetObjectAsJson(key, cart);
                }
            }

            bindingContext.Result = ModelBindingResult.Success(cart);
            return Task.CompletedTask;
        }
    }
}
