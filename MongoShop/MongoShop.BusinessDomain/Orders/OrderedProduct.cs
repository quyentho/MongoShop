using MongoShop.BusinessDomain.Products;
using MongoDB.Bson.Serialization.Attributes;
namespace MongoShop.BusinessDomain.Orders
{
    public class OrderedProduct
    {
        [BsonElement("product")] 
        public Product Product { get; set; }

        [BsonElement("Ordered_Quantity")]    
        public int OrderedQuantity { get; set; }

        [BsonElement("size")] 
        public string Size { get; set;}
    }
}
