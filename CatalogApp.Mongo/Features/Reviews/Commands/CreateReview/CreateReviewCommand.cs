using CatalogApp.Mongo.Entities;
using MediatR;

namespace CatalogApp.Mongo.Features.Reviews.Commands.CreateReview
{
    // Команда повертає створений об'єкт Review
    public class CreateReviewCommand : IRequest<Review>
    {
        public int ProductId { get; set; } // ID товару з SQL бази
        public string UserId { get; set; } // ID користувача (ObjectId як рядок)
        public string Title { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
    }
}