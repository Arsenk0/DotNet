using System;

namespace CatalogApp.Models
{
    public class ProductDetail
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string? Description { get; set; }
        public int PageCount { get; set; }
    }
}