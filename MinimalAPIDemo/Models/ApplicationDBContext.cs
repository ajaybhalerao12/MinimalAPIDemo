using Microsoft.EntityFrameworkCore;
using MinimalAPIDemo.Models.Products;

namespace MinimalAPIDemo.Models
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        // Table Name Products
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Add dummy data
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Laptop",
                    Price = 1200.00m,
                    Description = "High-performance laptop suitable for gaming and professional work"
                },
                new Product
                {
                    Id = 2,
                    Name = "Smartphone",
                    Price = 800.00m,
                    Description = "Latest model smartphone with high-resolution camera"
                },
                new Product
                {
                    Id = 3,
                    Name = "Headphones",
                    Price = 150.00m,
                    Description = "Noise-cancelling headphones with over 20 hours of battery life"
                }
                );
        }
    }
}
