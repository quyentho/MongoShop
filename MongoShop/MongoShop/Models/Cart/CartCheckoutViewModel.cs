using System.Collections.Generic;
using MongoShop.BusinessDomain.Orders;

namespace MongoShop.Models.Cart
{
    public class CartCheckoutViewModel
    {
        public List<OrderedProduct> Products { get; set; }
        public double Total { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string AddressNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Comment { get; set; }
    }
}
