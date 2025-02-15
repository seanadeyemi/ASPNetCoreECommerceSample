using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace ASPNetCoreECommerceSample.Models
{
    public class BannerModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public List<string> ImagePaths { get; set; }
        [DisplayName("Images")]
        public List<IFormFile> Images { get; set; } // Property for image uploads
    }
}
