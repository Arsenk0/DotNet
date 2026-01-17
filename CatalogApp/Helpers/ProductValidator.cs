using CatalogApp.Dtos;
using FluentValidation;

namespace CatalogApp.Helpers
{
    public class ProductValidator : AbstractValidator<ProductCreateDto>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Назва товару обов'язкова")
                .MaximumLength(100).WithMessage("Назва занадто довга");
            
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Ціна має бути більше 0");
            
            RuleFor(x => x.ProductCategoryId)
                .GreaterThan(0).WithMessage("Вкажіть категорію");
        }
    }
}