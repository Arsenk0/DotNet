using MediatR;
using System.Text.Json.Serialization;
namespace CatalogApp.Mongo.Features.Reviews.Commands.UpdateReview
{
    public class UpdateReviewCommand : IRequest<bool>
    {
        [JsonIgnore]
        public string Id { get; set; }
        public string NewText { get; set; }
        public int NewRating { get; set; }
    }
}