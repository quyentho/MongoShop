using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoShop.BusinessDomain.Categories;

namespace MongoShop.BusinessDomain.Products
{
    public class Product
    {

        public Product()
        {
            Images = new List<string>();
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
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

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
