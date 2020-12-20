using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace MongoShop.BusinessDomain.Product
{
    public class Product
    {
        [BsonId]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("price")]
        public double Price { get; set; }

        [BsonElement("stock_quantity")]
        public int StockQuantity { get; set; }

        [BsonElement("size")]
        public string Size { get; set; }

        [BsonElement("status")]
        public bool Status { get; set; }

        [BsonElement("images")]
        public List<string> Images { get; set;}

        public MongoDBRef Category { get; set; }
    }
}
