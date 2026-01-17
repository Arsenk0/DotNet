namespace CatalogApp.Specifications
{
    public class ProductSpecParams
    {
        private const int MaxPageSize = 50;
        
        // Пагінація
        public int PageIndex { get; set; } = 1;
        private int _pageSize = 6;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        // Фільтрація і пошук
        public int? CategoryId { get; set; }
        public string? Sort { get; set; } // priceAsc, priceDesc
        
        private string? _search;
        public string? Search 
        {
            get => _search;
            set => _search = value?.ToLower();
        }
    }
}