using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace MongoShop.BusinessDomain.Order
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("products")]
        public List<MongoDBRef> Products { get; set; }

        [BsonElement("users")]
        public List<MongoDBRef> Users { get; set; }

        [BsonElement("status")]
        public bool Status { get; set; }

        [BsonElement("grand_total")]
        public double Total { get; set; }

        [BsonElement("invoice")]
        public Invoice Invoice { get; set; }

    }

    public class Invoice
    {
        [BsonElement("payment_method")]
        public string PaymentMethod { get; set; }

        [BsonElement("status")]
        public string Status { get; set; }
    }
}
