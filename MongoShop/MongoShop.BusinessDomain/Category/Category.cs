﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoShop.BusinessDomain.Category
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }
    }
}
