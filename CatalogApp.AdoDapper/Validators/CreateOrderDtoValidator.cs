using CatalogApp.AdoDapper.Controllers;
using FluentValidation;

namespace CatalogApp.AdoDapper.Validators
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("Вкажіть ім'я клієнта")
                .MaximumLength(50);

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Замовлення не може бути пустим");

            RuleForEach(x => x.Items).ChildRules(items => 
            {
                items.RuleFor(x => x.ProductId).GreaterThan(0);
                items.RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Кількість має бути > 0");
            });
        }
    }
}