using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASPNetCoreECommerceSample.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LongDescription { get; set; }
        public decimal NormalPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public bool HasDiscount { get; set; } = false;

        public int Rating { get; set; }
        public int ReviewCount { get; set; }
        public List<ProductModel> RelatedProducts { get; set; }
        public string AdditionalInformation { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "Please select a category")] // Add Required attribute
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid category")] // Use Range to ensure the value is greater than 0
        public int SelectedCategoryId { get; set; } // Property to hold the selected category ID
        public IEnumerable<SelectListItem> Categories { get; set; } // List of available categories

        public List<string> ImagePaths { get; set; }
        [DisplayName("Images")]
        public List<IFormFile> Images { get; set; } // Property for image uploads
        public int Quantity { get; internal set; }

        [DisplayName("Available Colors")]
        public List<string> AvailableColors { get; set; }

        [DisplayName("Selected Colors")]
        public List<string> SelectedColors { get; set; } // New property to hold the selected colors

        [DisplayName("Available Sizes")]
        public List<string> AvailableSizes { get; set; }

        [DisplayName("Selected Sizes")]
        public List<string> SelectedSizes { get; set; } // New property to hold the selected colors


    }
}
