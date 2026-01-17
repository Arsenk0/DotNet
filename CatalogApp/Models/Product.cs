using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogApp.Models
{
    public class Product
    {
        public int Id { get; set; } // Використовуємо int, щоб не було конфліктів
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? PictureUrl { get; set; }

        // Зв'язок з категорією (Foreign Key)
        public int ProductCategoryId { get; set; }
        
        [ForeignKey("ProductCategoryId")]
        public Category? ProductCategory { get; set; }
    }
}