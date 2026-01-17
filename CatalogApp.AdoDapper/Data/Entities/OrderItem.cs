namespace CatalogApp.AdoDapper.Data.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; } // Додали ?
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}