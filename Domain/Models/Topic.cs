using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace it_explained.WebApi.Domain.Models
{
    public class Topic
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("slug")]
        public string? Slug { get; set; }

        [BsonElement("content")]
        public string? Content { get; set; }

        [BsonElement("tags")]
        public List<string>? Tags { get; set; }
    }
}
