using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoShop.BusinessDomain.Users
{
    public class Contacts
    {
        [BsonElement("contacts")]
        public List<Address> Addresses { get; set; }

        [BsonElement("phone_number")]
        public List<string> PhoneNumber { get; set; }
    }
}
