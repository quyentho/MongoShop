using MongoDB.Bson.Serialization.Attributes;

namespace MongoShop.BusinessDomain.Users
{
    public class Address
    {
        [BsonElement("number")]
        public string Number { get; set; }

        [BsonElement("street")]
        public string Street { get; set; }

        [BsonElement("city")]
        public string City { get; set; }

    }
}
