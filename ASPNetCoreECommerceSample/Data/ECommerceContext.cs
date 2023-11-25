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
        public DbSet<Banner> Banners { get; set; }
        public DbSet<BannerImage> BannerImages { get; set; }

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


            //modelBuilder.Entity<Banner>()
            //    .HasKey(p => p.Id);

            modelBuilder.Entity<ProductColor>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<ProductImage>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<ProductSize>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Banner>()
            .HasData
            (
            new Banner
            {
                Id = 1,
                Title = "Buchi Summer Collection",
                Description1 = "Exquisite Suits",
                Description2 = "Nice Jackets",
            },
            new Banner
            {
                Id = 2,
                Title = "Victor Designer wears",
                Description1 = "Blazers",
                Description2 = "Sneakers",
            }

            );
            modelBuilder.Entity<BannerImage>()
           .HasData
           (
                new BannerImage
                {
                    Id = 1,
                    BannerId = 1,
                    ImagePath = ""
                },
                new BannerImage
                {
                    Id = 2,
                    BannerId = 1,
                    ImagePath = ""
                }
                ,
               new BannerImage
               {
                   Id = 3,
                   BannerId = 2,
                   ImagePath = ""
               },
               new BannerImage
               {
                   Id = 4,
                   BannerId = 2,
                   ImagePath = ""
               }
           );
        }
    }
}
