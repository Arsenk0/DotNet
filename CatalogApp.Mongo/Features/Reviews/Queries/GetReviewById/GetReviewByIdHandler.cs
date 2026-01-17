using CatalogApp.Mongo.Entities;
using MediatR;
using MongoDB.Driver;

namespace CatalogApp.Mongo.Features.Reviews.Queries.GetReviewById
{
    public class GetReviewByIdHandler : IRequestHandler<GetReviewByIdQuery, Review>
    {
        private readonly IMongoCollection<Review> _reviews;

        public GetReviewByIdHandler(IMongoDatabase database)
        {
            _reviews = database.GetCollection<Review>("reviews");
        }

        public async Task<Review> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
        {
            // Шукаємо документ, де Id збігається з request.Id
            return await _reviews.Find(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        }
    }
}