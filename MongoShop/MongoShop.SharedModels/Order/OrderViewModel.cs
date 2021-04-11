using System.Collections.Generic;
using System.ComponentModel;

namespace MongoShop.SharedModels.Order
{
    public class OrderViewModel
    {

        public string Id { get; set; }

        [DisplayName("User Id")]
        public string UserId { get; set; }

        public List<OrderedProductViewModel> OrderedProducts { get; set; }

        public double Total { get; set; }

        [DisplayName("Payment")]
        public string InvoicePaymentMethod { get; set; }

        [DisplayName("Status")]
        public string InvoiceStatus { get; set; }

        public AddressViewModel ShipAddress { get; set; }

        [DisplayName("Phone number")]
        public string PhoneNumber { get; set; }

        public string Comment { get; set; }
    }
}