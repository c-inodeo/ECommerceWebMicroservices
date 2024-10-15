using Microsoft.EntityFrameworkCore;
using ECommerceWebMicroservices.Models;

namespace ProductCatalog.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasData(
               new Product
               {
                   Id = 1,
                   ProductName = "Car Wax",
                   Description = "High-performance car wax for a long-lasting shine.",
                   Price = 19.99,
               },
               new Product
               {
                   Id = 2,
                   ProductName = "Facial Cleanser",
                   Description = "Gentle facial cleanser for all skin types.",
                   Price = 12.49,
               },
               new Product
               {
                   Id = 3,
                   ProductName = "Mystery Novel",
                   Description = "A gripping mystery novel that will keep you on the edge of your seat.",
                   Price = 8.99,
               },
               new Product
               {
                   Id = 4,
                   ProductName = "Men's T-Shirt",
                   Description = "Comfortable cotton T-shirt available in various colors.",
                   Price = 15.00,
               },
               new Product
               {
                   Id = 5,
                   ProductName = "Smartphone",
                   Description = "Latest model with cutting-edge features.",
                   Price = 699.99,
               },
               new Product
               {
                   Id = 6,
                   ProductName = "Vitamins",
                   Description = "Daily multivitamins to boost your health.",
                   Price = 25.50,
               },
               new Product
               {
                   Id = 7,
                   ProductName = "Garden Tools Set",
                   Description = "Complete set of tools for your gardening needs.",
                   Price = 45.00,
               },
               new Product
               {
                   Id = 8,
                   ProductName = "Pet Bed",
                   Description = "Comfortable and durable pet bed for your furry friends.",
                   Price = 35.00,
               },
               new Product
               {
                   Id = 9,
                   ProductName = "Camping Tent",
                   Description = "Spacious tent for outdoor adventures.",
                   Price = 120.00,
               },
               new Product
               {
                   Id = 10,
                   ProductName = "Board Game",
                   Description = "Fun and engaging board game for family and friends.",
                   Price = 29.99,
               });
            Console.WriteLine("--> Done seeding data...");

        }
    }
}
