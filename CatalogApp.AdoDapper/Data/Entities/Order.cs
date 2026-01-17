namespace CatalogApp.AdoDapper.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; } // Додали ?
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "New";
        
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}