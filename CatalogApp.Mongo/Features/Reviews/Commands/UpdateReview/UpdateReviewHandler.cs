using CatalogApp.Mongo.Entities;
using MediatR;
using MongoDB.Driver;
namespace CatalogApp.Mongo.Features.Reviews.Commands.UpdateReview
{
    public class UpdateReviewHandler : IRequestHandler<UpdateReviewCommand, bool>
    {
        private readonly IMongoCollection<Review> _reviews;
        public UpdateReviewHandler(IMongoDatabase db) => _reviews = db.GetCollection<Review>("reviews");

        public async Task<bool> Handle(UpdateReviewCommand request, CancellationToken ct)
        {
            var update = Builders<Review>.Update
                .Set(r => r.Text, request.NewText)
                .Set(r => r.Rating, request.NewRating);

            var result = await _reviews.UpdateOneAsync(r => r.Id == request.Id, update, cancellationToken: ct);
            return result.ModifiedCount > 0;
        }
    }
}