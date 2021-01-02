using System;
using System.Collections.Generic;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;
using MongoShop.BusinessDomain.Carts;

namespace MongoShop.BusinessDomain.Users
{
    [CollectionName("user")]
    public class ApplicationUser : MongoIdentityUser<Guid>
    {
        public ApplicationUser() : base()
        {

        }

        public ApplicationUser(string userName, string email) : base(userName, email)
        {

        }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("status")]
        public bool Status { get; set; }

        [BsonElement("contacts")]
        public List<Contacts> Contacts { get; set; }

        [BsonElement("cart")]
        public Cart Cart { get; set; } = new Cart();
    }
}
