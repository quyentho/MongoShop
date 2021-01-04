using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoShop.BusinessDomain.Orders;

namespace MongoShop.Models.Cart
{
    public class CartIndexViewModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public List<OrderedProduct> Products { get; set; }

        public double Total { get; set; }
    }
}
