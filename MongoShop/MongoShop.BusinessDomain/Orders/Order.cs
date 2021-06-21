using System;
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

        [BsonElement("address")]
        public Address ShipAddress { get; set; }

        [BsonElement("phone_number")]
        public string PhoneNumber { get; set; }

        [BsonElement("comment")]
        public string Comment { get; set; }

        [BsonElement("created_time")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedTime { get; set; }
    }
}
