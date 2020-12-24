using System;
using System.Collections.Generic;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDbGenericRepository.Attributes;

namespace MongoShop.BusinessDomain.User
{
    [CollectionName("user")]
    public class ApplicationUser : MongoIdentityUser<Guid>
    {
        public ApplicationUser() :base()
        {

        }

        public ApplicationUser(string userName, string email): base(userName,email)
        {

        }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("status")]
        public bool Status { get; set; }

        [BsonElement("contacts")]
        public List<Contacts> Contacts { get; set; }

        [BsonElement("cart")]
        public Cart Cart { get; set; }
    }

    public class Cart
    {
        List<MongoDBRef> Products { get; set; }
    }
}
