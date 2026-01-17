using CatalogApp.Mongo.Entities;
using MediatR;
using MongoDB.Driver;

namespace CatalogApp.Mongo.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewHandler : IRequestHandler<CreateReviewCommand, Review>
    {
        private readonly IMongoCollection<Review> _reviews;

        public CreateReviewHandler(IMongoDatabase database)
        {
            _reviews = database.GetCollection<Review>("reviews");
        }

        public async Task<Review> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            // 1. Мапимо (перетворюємо) команду в сутність бази даних
            var newReview = new Review
            {
                // MongoDB драйвер сам згенерує Id при вставці, або можна тут:
                // Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                
                ProductId = request.ProductId,
                UserId = request.UserId,
                Title = request.Title,
                Text = request.Text,
                Rating = request.Rating,
                CreatedAt = DateTime.UtcNow, // Дата ставиться сервером
                Comments = new List<Comment>() // Створюємо пустий список коментарів
            };

            // 2. Зберігаємо в базу
            await _reviews.InsertOneAsync(newReview, cancellationToken: cancellationToken);

            // 3. Повертаємо створений об'єкт
            return newReview;
        }
    }
}