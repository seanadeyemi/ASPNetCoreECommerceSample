namespace ASPNetCoreECommerceSample.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<ShoppingCartItem> Items { get; set; }
    }

}
