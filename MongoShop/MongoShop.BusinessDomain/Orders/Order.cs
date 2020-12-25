using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace MongoShop.BusinessDomain.Orders
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("products")]
        public List<string> Products { get; set; }

        [BsonElement("users")]
        public List<string> Users { get; set; }

        [BsonElement("grand_total")]
        public double Total { get; set; }

        [BsonElement("invoice")]
        public Invoice Invoice { get; set; }

    }

   
}
