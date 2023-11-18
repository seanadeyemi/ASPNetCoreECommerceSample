using ASPNetCoreECommerceSample.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASPNetCoreECommerceSample.Data
{
    public class ECommerceContext : DbContext
    {
        public ECommerceContext(DbContextOptions<ECommerceContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().
                HasKey(p => p.Id);


            modelBuilder.Entity<ProductColor>()
                .HasOne(c => c.Product)
                .WithMany(p => p.AvailableColors)
                .HasForeignKey(c => c.ProductId);


            modelBuilder.Entity<ProductSize>()
                  .HasOne(c => c.Product)
                .WithMany(p => p.AvailableSizes)
                .HasForeignKey(c => c.ProductId);
        }
    }
}
