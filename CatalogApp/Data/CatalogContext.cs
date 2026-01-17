using CatalogApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogApp.Data
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Налаштування для Категорії
            modelBuilder.Entity<Category>().HasKey(c => c.Id);

            // Налаштування для Товару
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            // Налаштування зв'язку (1 Категорія -> Багато Товарів)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductCategory)
                .WithMany()
                .HasForeignKey(p => p.ProductCategoryId);
        }
    }
}