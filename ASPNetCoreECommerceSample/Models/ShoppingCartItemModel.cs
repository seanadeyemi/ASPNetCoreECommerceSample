namespace ASPNetCoreECommerceSample.Models
{
    public class ShoppingCartItemModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public List<string> ProductImagePaths { get; set; } = new List<string>();
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
