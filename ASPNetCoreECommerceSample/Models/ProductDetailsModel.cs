namespace ASPNetCoreECommerceSample.Models
{
    public class ProductDetailsModel
    {
        public ProductModel Product { get; set; }
        public List<ReviewViewModel> Reviews { get; set; }
        //public List<RelatedProductViewModel> RelatedProducts { get; set; }
        public List<RelatedProductViewModel> RelatedProducts { get; set; }
    }
}
