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
            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Product>()
      .HasMany(p => p.Reviews)
      .WithOne(r => r.Product)
      .HasForeignKey(r => r.ProductId)
      .OnDelete(DeleteBehavior.Cascade);

            // Configure the many-to-many relationship for related products
            modelBuilder.Entity<ProductProduct>()
                .HasKey(pp => new { pp.ProductId, pp.RelatedProductId });

            //modelBuilder.Entity<ProductProduct>()
            //    .HasOne(pp => pp.Product)
            //    .WithMany(p => p.Products)
            //    .HasForeignKey(pp => pp.ProductId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<ProductProduct>()
            //    .HasOne(pp => pp.RelatedProduct)
            //    .WithMany(p => p.RelatedProducts)
            //    .HasForeignKey(pp => pp.RelatedProductId)
            //    .OnDelete(DeleteBehavior.Restrict);

            // Configure the many-to-many relationship for related products
            modelBuilder.Entity<ProductProduct>()
                .HasKey(pp => new { pp.ProductId, pp.RelatedProductId });

            modelBuilder.Entity<ProductProduct>()
                .HasOne(pp => pp.Product)
                .WithMany(p => p.RelatedProducts)
                .HasForeignKey(pp => pp.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductProduct>()
                .HasOne(pp => pp.RelatedProduct)
                .WithMany(p => p.Products)
                .HasForeignKey(pp => pp.RelatedProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.AvailableColors)
                .WithOne()
                .IsRequired();

            modelBuilder.Entity<Product>()
                .HasMany(p => p.AvailableSizes)
                .WithOne()
                .IsRequired();

            modelBuilder.Entity<Product>()
             .HasMany(p => p.Reviews)
             .WithOne(r => r.Product)
             .HasForeignKey(r => r.ProductId)
             .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<ProductCategory>()
                .HasKey(pc => new { pc.ProductId, pc.CategoryId });

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(pc => pc.ProductId);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(pc => pc.CategoryId);

            // Configuring Review entity
            modelBuilder.Entity<Review>()
                .HasKey(r => r.ReviewId);

            modelBuilder.Entity<Review>()
       .HasMany(r => r.Replies)
       .WithOne(r => r.ParentReview)
       .HasForeignKey(r => r.ParentReviewId)
       .OnDelete(DeleteBehavior.NoAction);
            ;

            //modelBuilder.Entity<ProductColor>()
            //    .HasMany(p => p.AvailableColors)
            //    .WithMany()
            //    .HasForeignKey(c => c.ProductId);


            //modelBuilder.Entity<ProductSize>()
            //      .HasOne(c => c.Product)
            //    .WithMany(p => p.AvailableSizes)
            //    .HasForeignKey(c => c.ProductId);


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
