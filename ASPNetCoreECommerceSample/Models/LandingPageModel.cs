namespace ASPNetCoreECommerceSample.Models
{
    public class LandingPageModel
    {
        public List<BannerModel> Banners { get; set; }
        public List<ProductModel> BestSellerProducts { get; set; }
        public List<ProductModel> NewArrivalProducts { get; set; }
        public List<ProductModel> SaleItemProducts { get; set; }
    }
}
