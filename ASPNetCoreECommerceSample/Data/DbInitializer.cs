using ASPNetCoreECommerceSample.Entities;

namespace ASPNetCoreECommerceSample.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ECommerceContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Categories.Any())
            {



                var categories = new Category[]
                {
            new Category { Name = "Computing" },
            new Category { Name = "Computers", ParentCategoryId = 1 },
            new Category { Name = "Data Storage", ParentCategoryId = 1 },
            new Category { Name = "Printers", ParentCategoryId = 1 },
            new Category { Name = "Computer accessories", ParentCategoryId = 1 },
            new Category { Name = "Desktops", ParentCategoryId = 2 },
            new Category { Name = "Laptops", ParentCategoryId = 2 },
            new Category { Name = "Macbooks", ParentCategoryId = 2 },
                };

                foreach (var c in categories)
                {
                    context.Categories.Add(c);
                }
                context.SaveChanges();
            }

        }
    }

}
