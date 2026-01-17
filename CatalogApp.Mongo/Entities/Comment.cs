using MongoDB.Bson; // <--- Не забудьте цей using
using MongoDB.Bson.Serialization.Attributes;

namespace CatalogApp.Mongo.Entities
{
    [BsonIgnoreExtraElements] // <--- Цей атрибут врятує від помилок, якщо в базі є зайві поля
    public class Comment
    {
        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UserId { get; set; } // <--- Ми додали це поле, бо воно є в базі

        [BsonElement("text")]
        public string? Text { get; set; }

        [BsonElement("postedAt")]
        public DateTime PostedAt { get; set; }
    }
}