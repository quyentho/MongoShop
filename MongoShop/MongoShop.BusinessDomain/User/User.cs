using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace MongoShop.BusinessDomain.User
{
    public class User
    {
        [BsonId]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("permission")]
        public string Permission { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("status")]
        public bool Status { get; set; }

        [BsonElement("external_login")]
        public List<ExternalLogin> ExternalLogin { get; set; }

        [BsonElement("contacts")]
        public List<Contacts> Contacts { get; set; }

        [BsonElement("cart")]
        public Cart Cart { get; set; }

        [BsonElement("reset_password_token")]
        public string ResetPasswordToken { get; set; }
    }

    public class Cart
    {
        List<MongoDBRef> Products { get; set; }
    }
}
