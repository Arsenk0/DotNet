using MediatR;
namespace CatalogApp.Mongo.Features.Reviews.Commands.DeleteReview
{
    public record DeleteReviewCommand(string Id) : IRequest<bool>;
}