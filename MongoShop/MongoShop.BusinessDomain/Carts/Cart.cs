using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoShop.BusinessDomain.Products;

namespace MongoShop.BusinessDomain.Carts
{
    public class Cart
    {
        public Cart()
        {
            Id = ObjectId.GenerateNewId().ToString();
            Products = new List<Product>(); 
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public List<Product> Products { get; set; }
    }
}
