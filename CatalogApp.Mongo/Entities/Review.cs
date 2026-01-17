using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CatalogApp.Mongo.Entities
{
    public class Review
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("productId")]
        public int ProductId { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UserId { get; set; }

        [BsonElement("title")]
        public string? Title { get; set; }

        [BsonElement("text")]
        public string? Text { get; set; }

        [BsonElement("rating")]
        public int Rating { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("comments")]
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}