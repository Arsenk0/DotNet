using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CatalogApp.Mongo.Entities
{
    public class ProductRating
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } // Додали ?

        [BsonElement("productId")]
        public int ProductId { get; set; }

        [BsonElement("averageRating")]
        public double AverageRating { get; set; }

        [BsonElement("reviewsCount")]
        public int ReviewsCount { get; set; }
    }
}