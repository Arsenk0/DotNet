using AutoMapper;
using CatalogApp.Dtos;
using CatalogApp.Models;

namespace CatalogApp.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>()
                // Додали перевірку: якщо категорія є - беремо ім'я, якщо ні - пустий рядок
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.ProductCategory != null ? s.ProductCategory.Name : string.Empty));

            CreateMap<ProductCreateDto, Product>();
        }
    }
}