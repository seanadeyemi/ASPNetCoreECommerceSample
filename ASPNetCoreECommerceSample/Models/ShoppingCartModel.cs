namespace ASPNetCoreECommerceSample.Models
{
    public class ShoppingCartModel
    {
        public int CustomerId { get; set; }
        public string ReturnUrl { get; set; }
        public List<ShoppingCartItemModel> Items { get; set; } = new List<ShoppingCartItemModel>();
        public decimal TotalPrice => Items.Sum(i => i?.TotalPrice ?? 0);

        public void AddItem(ShoppingCartItemModel item)
        {
            Items.Add(item);
        }

        public void RemoveItem(ShoppingCartItemModel item)
        {
            Items.Remove(item);
        }
    }
}
