using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoShop.BusinessDomain.Users
{
    public class Contact
    {
        [BsonElement("address")]
        public Address Address { get; set; }

        [BsonElement("phone_number")]
        public string PhoneNumber { get; set; }
    }
}
