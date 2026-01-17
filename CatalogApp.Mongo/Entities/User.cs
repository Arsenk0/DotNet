using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CatalogApp.Mongo.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } // Додали ?

        [BsonElement("name")]
        public string? Name { get; set; } // Додали ?

        [BsonElement("email")]
        public string? Email { get; set; } // Додали ?
    }
}