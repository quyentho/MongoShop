using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoShop.BusinessDomain.Categories
{
    public class UploadedImage
    {
        [BsonElement("path")]
        public string ImagePath { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
