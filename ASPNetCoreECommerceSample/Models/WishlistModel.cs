namespace ASPNetCoreECommerceSample.Models
{
    public class WishlistModel
    {
        public string CustomerId { get; set; }
        public string ReturnUrl { get; set; }
        public List<WishlistItemModel> WishlistItems { get; set; } = new List<WishlistItemModel>();
        public decimal TotalPrice => WishlistItems.Sum(i => i?.TotalPrice ?? 0);

        public void AddItem(WishlistItemModel item)
        {
            WishlistItems.Add(item);
        }

        public void RemoveItem(WishlistItemModel item)
        {
            WishlistItems.Remove(item);
        }
    }
}
