using System.ComponentModel.DataAnnotations;

namespace ASPNetCoreECommerceSample.Entities
{
    public class ProductColor
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
