using CatalogApp.Mongo.Entities;
using MediatR;

namespace CatalogApp.Mongo.Features.Reviews.Queries.GetReviewById
{
    // Цей запит повертає ОДИН Review
    public class GetReviewByIdQuery : IRequest<Review>
    {
        public string Id { get; }

        public GetReviewByIdQuery(string id)
        {
            Id = id;
        }
    }
}