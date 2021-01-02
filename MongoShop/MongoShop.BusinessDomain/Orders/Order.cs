using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoShop.BusinessDomain.Users;

namespace MongoShop.BusinessDomain.Orders
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("products")]
        public List<OrderedProduct> OrderedProducts { get; set; }

        [BsonElement("user_id")]
        public string UserId { get; set; }

        [BsonElement("grand_total")]
        public double Total { get; set; }

        [BsonElement("invoice")]
        public Invoice Invoice { get; set; }
    }
}
