using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AppMongoDB.Models
{
    public class Author
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string fullName { get; set; }
        
        public string country { get; set; }

    }
}
