using MongoShop.BusinessDomain.Products;

namespace MongoShop.BusinessDomain.Orders
{
    public class OrderedProduct
    {
        public Product Product { get; set; }

        public int OrderedQuantity { get; set; }
    }
}
