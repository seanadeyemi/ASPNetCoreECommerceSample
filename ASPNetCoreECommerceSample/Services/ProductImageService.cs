using ASPNetCoreECommerceSample.Data;
using ASPNetCoreECommerceSample.Entities;

namespace ASPNetCoreECommerceSample.Services
{
    public interface IProductImageService
    {
        List<ProductImage> GetAllProductimages();
    }

    public class ProductImageService : IProductImageService
    {
        private readonly ECommerceContext _context;

        public ProductImageService(ECommerceContext context)
        {
            _context = context;
        }

        public List<ProductImage> GetAllProductimages()
        {
            var productImageList = _context.ProductImages
                    .OrderBy(p => p.Id)
            .ToList();

            //productImageList = productImageList.Select(b =>
            //{
            //    b.BannerImages = _context.BannerImages.Where(pi => pi.BannerId == b.Id).ToList();
            //    return b;
            //}).ToList();



            return productImageList;
            // return Enumerable.Empty<Banner>().ToList();
        }
    }
}
