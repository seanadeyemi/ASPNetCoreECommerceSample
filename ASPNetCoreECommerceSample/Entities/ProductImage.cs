using System.ComponentModel.DataAnnotations;


namespace ASPNetCoreECommerceSample.Entities;

public class ProductImage
{
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    [StringLength(255)] // Adjust the maximum length as needed
    public string ImagePath { get; set; }
}
