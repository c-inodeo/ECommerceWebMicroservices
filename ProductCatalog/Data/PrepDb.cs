using ECommerceWebMicroservices.Models;

namespace ProductCatalog.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app) 
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<ApplicationDbContext>());
            }
        }
        private static void SeedData(ApplicationDbContext context)
        {
            if (!context.Products.Any())
            {
                Console.WriteLine("===Seeding Data (PrepDB) !===");
                context.Products.AddRange(
                        new Product() { ProductName="Sample", Description ="PrepDb",  Price = 777},
                        new Product() { ProductName = "PrepDb", Description = "PrepDb", Price = 111 },
                        new Product() { ProductName = "This is from PrepDb", Description = "PrepDb", Price = 444 }
                 );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("===We already have data!===");
            }
        }
    }
}
