using CatalogApp.Models;

namespace CatalogApp.Specifications
{
    public class ProductWithCategorySpecification : BaseSpecification<Product>
    {
        // Конструктор для фільтрації, пошуку та пагінації
        public ProductWithCategorySpecification(ProductSpecParams productParams) 
            : base(x => 
                (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
                (!productParams.CategoryId.HasValue || x.ProductCategoryId == productParams.CategoryId)
            )
        {
            AddInclude(x => x.ProductCategory);
            ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(n => n.Name);
                        break;
                }
            }
        }

        // Конструктор для отримання одного товару по ID
        public ProductWithCategorySpecification(int id) 
            : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductCategory);
        }
    }
}