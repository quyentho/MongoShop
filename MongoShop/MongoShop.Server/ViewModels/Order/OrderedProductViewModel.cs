using MongoShop.Areas.Admin.ViewModels.Product;

namespace MongoShop.Server.ViewModels.Order
{
    public class OrderedProductViewModel
    {
        public ProductViewModel Product { get; set; }

        public int OrderedQuantity { get; set; }
    }
}