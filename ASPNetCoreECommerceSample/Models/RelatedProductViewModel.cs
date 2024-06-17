namespace ASPNetCoreECommerceSample.Models
{
    public class RelatedProductViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal DiscountPrice { get; set; }
        public string Image { get; set; }
        public bool HasDiscount { get; set; }
        public int Rating { get; set; }
        public List<string> ImagePaths { get; set; }
        public int Id { get; internal set; }
        public decimal NormalPrice { get; internal set; }
        public string Description { get; internal set; }
    }
}
