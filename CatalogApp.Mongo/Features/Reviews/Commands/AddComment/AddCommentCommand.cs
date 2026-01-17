using MediatR;
using System.Text.Json.Serialization; // Для ігнорування ReviewId в JSON body

namespace CatalogApp.Mongo.Features.Reviews.Commands.AddComment
{
    // Повертаємо bool (успішно чи ні)
    public class AddCommentCommand : IRequest<bool>
    {
        [JsonIgnore] // Це поле ми візьмемо з URL, а не з тіла запиту
        public string? ReviewId { get; set; }

        public string UserId { get; set; } // Хто пише коментар
        public string Text { get; set; }
    }
}