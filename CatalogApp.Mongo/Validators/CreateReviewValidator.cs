using CatalogApp.Mongo.Features.Reviews.Commands.CreateReview;
using FluentValidation;

namespace CatalogApp.Mongo.Validators
{
    public class CreateReviewValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Rating).InclusiveBetween(1, 5).WithMessage("Рейтинг має бути від 1 до 5");
            RuleFor(x => x.UserId).Length(24).WithMessage("Невірний ID користувача");
        }
    }
}