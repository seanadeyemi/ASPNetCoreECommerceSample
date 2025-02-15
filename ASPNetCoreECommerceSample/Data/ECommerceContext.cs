using ASPNetCoreECommerceSample.Entities;
using ASPNetCoreECommerceSample.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASPNetCoreECommerceSample.Data
{
    public class ECommerceContext : IdentityDbContext<ApplicationUser>
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

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            var hasher = new PasswordHasher<ApplicationUser>();

            //create a role
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", Name = "Admin", NormalizedName = "ADMIN" });

            //create a user
            modelBuilder.Entity<ApplicationUser>().HasData(
               new ApplicationUser
               {
                   Id = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                   UserName = "ecommerceadmin",
                   NormalizedUserName = "ECOMMERCEADMIN",
                   PasswordHash = hasher.HashPassword(null, "Pa$$w0rd"),
                   FirstName = "Admin",
                   LastName = "Admin"

               }
               );

            //asign admin role to the user we created
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"

            });

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


     


            modelBuilder.Entity<Category>()
           .HasOne(c => c.ParentCategory)
           .WithMany(c => c.SubCategories)
           .HasForeignKey(c => c.ParentCategoryId).IsRequired(false);


            //modelBuilder.Entity<ProductCategory>()
            //    .HasKey(pc => new { pc.ProductId, pc.CategoryId });

            //modelBuilder.Entity<ProductCategory>()
            //    .HasOne(pc => pc.Product)
            //    .WithMany(p => p.ProductCategories)
            //    .HasForeignKey(pc => pc.ProductId);

            //modelBuilder.Entity<ProductCategory>()
            //    .HasOne(pc => pc.Category)
            //    .WithMany(c => c.ProductCategories)
            //    .HasForeignKey(pc => pc.CategoryId);

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


            // Order
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.ShippingAddress)
                .WithMany()
                .HasForeignKey(o => o.ShippingAddressId)
                .OnDelete(DeleteBehavior.NoAction);

            // OrderItem
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                ;

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);


            modelBuilder.Entity<Wishlist>()
                .HasMany(p => p.WishlistItems)
                .WithOne(p => p.Wishlist)
                .HasForeignKey(p => p.WishlistId);



            // Customer
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Addresses)
                .WithOne(a => a.Customer)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);



            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }

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
