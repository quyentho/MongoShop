using System.Collections.Generic;
using MongoShop.BusinessDomain.Orders;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MongoShop.Models.Cart
{
    public class CartCheckoutViewModel
    {
        public List<OrderedProduct> Products { get; set; }

        [Required(ErrorMessage="Tên người nhận là bắt buôc")]
        public string RecipientName {get;set;}
        public double? Total { get; set; }

        [Required(ErrorMessage="Tên đường là bắt buộc")]
        public string Street { get; set; }

        [Required(ErrorMessage="Thành phố là bắt buộc")]
        public string City { get; set; }
        [Required(ErrorMessage="Số nhàlà bắt buôc")]
        public string AddressNumber { get; set; }

        [Required(ErrorMessage="Số điện thoại")]
        public string PhoneNumber { get; set; }

        public string Comment{get;set;}
    }
}
