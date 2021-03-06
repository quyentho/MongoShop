﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoShop.BusinessDomain.Categories
{
    public class Category
    {
        public Category()
        {

        }
        public Category(string name)
        {
            Name = name;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        public bool IsMainCate { get; set; }

        public bool Status { get; set; }

        public string ParentId { get; set; }
    }
}
