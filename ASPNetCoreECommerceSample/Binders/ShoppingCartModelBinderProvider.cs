using ASPNetCoreECommerceSample.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace ASPNetCoreECommerceSample.Binders
{

    public class ShoppingCartModelBinderProvider : IModelBinderProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShoppingCartModelBinderProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(ShoppingCartModel))
            {
                return new BinderTypeModelBinder(typeof(ShoppingCartModelBinder));
            }

            return null;
        }
    }
}
