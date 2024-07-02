using ASPNetCoreECommerceSample.Data;
using ASPNetCoreECommerceSample.Entities;

namespace ASPNetCoreECommerceSample.Services
{
    public interface IProductImageService
    {
        List<ProductImage> GetAllProductimages();
        Task<List<byte[]>> GetAllProductThumbnailImages(int productId, int width, int height);
        Task<byte[]> GetProductThumbnailImage(string serverPath, int productId, int width, int height);
    }

    public class ProductImageService : IProductImageService
    {
        private readonly ECommerceContext _context;
        private readonly IImageService _imageService;

        public ProductImageService(ECommerceContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
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

        public async Task<List<byte[]>> GetAllProductThumbnailImages(int productId, int width, int height)
        {
            var imagePaths = _context.ProductImages
                 .Where(p => p.ProductId
                 == productId).Select(c => c.ImagePath).ToList();

            var imageBytesTasks = imagePaths.Select(async imagePath => await System.IO.File.ReadAllBytesAsync(imagePath));

            var imageBytesList = await Task.WhenAll(imageBytesTasks);

            var thumbnailBytesList = imageBytesList.Select(imageBytes => _imageService.ResizeImage(imageBytes, width, height)).ToList();


            return thumbnailBytesList;
        }
        public async Task<byte[]> GetProductThumbnailImage(string serverPath, int productId, int width, int height)
        {
            var imagePath = _context.ProductImages
                 .Where(p => p.ProductId
                 == productId).Select(c => c.ImagePath).FirstOrDefault();
            if (imagePath is null)
                return default;

            var filePath = Path.Combine(serverPath, imagePath);
            var imageBytes = await System.IO.File.ReadAllBytesAsync(filePath);



            var thumbnailBytes = _imageService.ResizeImage(imageBytes, width, height);


            return thumbnailBytes;
        }

    }
}

