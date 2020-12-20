using MongoDB.Bson.Serialization.Attributes;

namespace MongoShop.BusinessDomain.Category
{
    public class Category
    {
        [BsonId]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }
    }
}
