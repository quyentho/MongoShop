using System.ComponentModel;

namespace MongoShop.Server.ViewModels.Order
{
    public class OrderViewModel
    {

        public string Id { get; set; }

        public string UserId { get; set; }


        public double Total { get; set; }

        [DisplayName("Payment")]
        public string InvoicePaymentMethod { get; set; }

        [DisplayName("Status")]
        public string InvoiceStatus { get; set; }
    }
}