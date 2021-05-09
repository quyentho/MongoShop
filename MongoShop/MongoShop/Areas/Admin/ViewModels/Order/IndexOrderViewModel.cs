using System;
using System.ComponentModel;

namespace MongoShop.Areas.Admin.ViewModels.Order
{
    public class IndexOrderViewModel
    {

        public string Id { get; set; }

        public string UserId { get; set; }


        public double Total { get; set; }

        [DisplayName("Payment")]
        public string InvoicePaymentMethod { get; set; }

        [DisplayName("Status")]
        public string InvoiceStatus { get; set; }

        [DisplayName("Created_Time")]
        public DateTime CreatedAt { get; set; }
    }
}