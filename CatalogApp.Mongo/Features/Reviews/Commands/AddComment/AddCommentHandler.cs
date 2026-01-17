using CatalogApp.Mongo.Entities;
using MediatR;
using MongoDB.Driver;

namespace CatalogApp.Mongo.Features.Reviews.Commands.AddComment
{
    public class AddCommentHandler : IRequestHandler<AddCommentCommand, bool>
    {
        private readonly IMongoCollection<Review> _reviews;

        public AddCommentHandler(IMongoDatabase database)
        {
            _reviews = database.GetCollection<Review>("reviews");
        }

        public async Task<bool> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            // 1. Створюємо об'єкт коментаря
            var comment = new Comment
            {
                UserId = request.UserId,
                Text = request.Text,
                PostedAt = DateTime.UtcNow
            };

            // 2. Створюємо фільтр: знайти відгук за ID
            var filter = Builders<Review>.Filter.Eq(x => x.Id, request.ReviewId);

            // 3. Створюємо оновлення: PUSH у масив Comments
            var update = Builders<Review>.Update.Push(x => x.Comments, comment);

            // 4. Виконуємо оновлення
            var result = await _reviews.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

            // Повертаємо true, якщо хоч один документ було змінено
            return result.ModifiedCount > 0;
        }
    }
}