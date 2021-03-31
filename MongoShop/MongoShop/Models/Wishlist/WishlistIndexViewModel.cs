using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoShop.BusinessDomain.Orders;
using MongoShop.BusinessDomain.Products;

namespace MongoShop.Models.Wishlist
{
    public class WishlistIndexViewModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public List<Product> Products { get; set; }
    }
}
