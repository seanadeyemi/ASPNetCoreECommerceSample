using System.ComponentModel.DataAnnotations.Schema;

namespace ASPNetCoreECommerceSample.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public int? ShippingAddressId { get; set; }
        public Address ShippingAddress { get; set; }
    }

}
