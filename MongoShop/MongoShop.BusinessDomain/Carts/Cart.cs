using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoShop.BusinessDomain.Orders;
using MongoShop.BusinessDomain.Products;

namespace MongoShop.BusinessDomain.Carts
{
    public class Cart
    {
        public Cart()
        {
            Id = ObjectId.GenerateNewId().ToString();
            Total = 0;
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Products")]
        public List<OrderedProduct> Products { get; set; } = new List<OrderedProduct>();

        public double? Total { get; set; }
    }
}
