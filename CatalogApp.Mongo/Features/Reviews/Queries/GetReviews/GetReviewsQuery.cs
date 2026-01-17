using CatalogApp.Mongo.Entities;
using MediatR;

namespace CatalogApp.Mongo.Features.Reviews.Queries.GetReviews
{
    // Запит повертає список відгуків
    public class GetReviewsQuery : IRequest<IEnumerable<Review>>
    {
    }
}