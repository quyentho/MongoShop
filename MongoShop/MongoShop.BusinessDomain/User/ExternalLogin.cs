using MongoDB.Bson.Serialization.Attributes;

namespace MongoShop.BusinessDomain.User
{
    public class ExternalLogin
    {
        [BsonElement("provider_key")]
        public string ProviderKey { get; set; }

        [BsonElement("provider_name")]
        public string ProviderName { get; set; }
    }
}
