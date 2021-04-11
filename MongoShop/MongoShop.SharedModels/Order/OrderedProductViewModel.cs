using MongoShop.SharedModels.Product;

namespace MongoShop.SharedModels.Order
{
    public class OrderedProductViewModel
    {
        public ProductViewModel Product { get; set; }

        public int OrderedQuantity { get; set; }
    }
}