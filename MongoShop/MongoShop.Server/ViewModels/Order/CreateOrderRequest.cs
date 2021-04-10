using MongoShop.BusinessDomain.Users;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MongoShop.Server.ViewModels.Order
{
    public class CreateOrderRequest
    {
        [Required]
        [DisplayName("User Id")]
        public string UserId { get; set; }

        [Required]
        public List<CreateOrderedProductRequest> OrderedProducts { get; set; }

        [Range(0, 1_000_000_000)]
        public double Total { get; set; }

        [DisplayName("Payment Method")]
        public string InvoicePaymentMethod { get; set; }

        [DisplayName("Status")]

        [Required]
        public string InvoiceStatus { get; set; }

        [Required]
        public Address ShipAddress { get; set; }

        [DisplayName("Phone number")]
        [Required]
        public string PhoneNumber { get; set; }

        public string Comment { get; set; }
    }
}
