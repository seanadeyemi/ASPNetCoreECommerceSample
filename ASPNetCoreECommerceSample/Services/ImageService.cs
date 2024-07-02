
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;


namespace ASPNetCoreECommerceSample.Services
{


    public interface IImageService
    {
        byte[] ResizeImage(byte[] imageBytes, int width, int height);
    }

    public class ImageService : IImageService
    {
        public byte[] ResizeImage(byte[] imageBytes, int width, int height)
        {
            using (var image = Image.Load(imageBytes, out IImageFormat format))
            {
                image.Mutate(x => x.Resize(width, height));
                using (var ms = new MemoryStream())
                {
                    image.Save(ms, format);
                    return ms.ToArray();
                }
            }
        }
    }

}
