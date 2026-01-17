using CatalogApp.Mongo.Entities;
using MediatR;
using MongoDB.Driver;
namespace CatalogApp.Mongo.Features.Reviews.Commands.DeleteReview
{
    public class DeleteReviewHandler : IRequestHandler<DeleteReviewCommand, bool>
    {
        private readonly IMongoCollection<Review> _reviews;
        public DeleteReviewHandler(IMongoDatabase db) => _reviews = db.GetCollection<Review>("reviews");

        public async Task<bool> Handle(DeleteReviewCommand request, CancellationToken ct)
        {
            var result = await _reviews.DeleteOneAsync(r => r.Id == request.Id, ct);
            return result.DeletedCount > 0;
        }
    }
}