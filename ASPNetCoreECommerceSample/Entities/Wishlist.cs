namespace ASPNetCoreECommerceSample.Entities
{
    public class Wishlist
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<WishlistItem> WishlistItems { get; set; }
    }
}
