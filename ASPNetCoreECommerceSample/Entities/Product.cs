﻿namespace ASPNetCoreECommerceSample.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal NormalPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public string LongDescription { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }

        public virtual List<ProductColor?>? AvailableColors { get; set; }
        public virtual List<ProductSize?>? AvailableSizes { get; set; }

        public DateTime DateAdded { get; set; }
        public DateTime? DateModified { get; set; }
        public int? Rating { get; internal set; }
    }
}