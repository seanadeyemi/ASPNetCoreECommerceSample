namespace ASPNetCoreECommerceSample.Entities
{
    public class ProductSize
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
