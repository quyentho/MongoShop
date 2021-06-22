using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoShop.BusinessDomain.Orders;

namespace MongoShop.Models.Cart
{
    public class CartCheckoutViewModel
    {
        public List<OrderedProduct> Products { get; set; }
        public double? Total { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string AddressNumber { get; set; }
        [Required]
        public string PhoneNumber { get; set; }

        public string Comment { get; set; }
    }
}
