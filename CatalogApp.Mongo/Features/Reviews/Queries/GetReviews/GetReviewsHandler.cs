using CatalogApp.Mongo.Entities;
using MediatR;
using MongoDB.Driver;

namespace CatalogApp.Mongo.Features.Reviews.Queries.GetReviews
{
    public class GetReviewsHandler : IRequestHandler<GetReviewsQuery, IEnumerable<Review>>
    {
        private readonly IMongoCollection<Review> _reviews;

        public GetReviewsHandler(IMongoDatabase database)
        {
            _reviews = database.GetCollection<Review>("reviews");
        }

        public async Task<IEnumerable<Review>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
        {
            return await _reviews.Find(_ => true).ToListAsync(cancellationToken);
        }
    }
}