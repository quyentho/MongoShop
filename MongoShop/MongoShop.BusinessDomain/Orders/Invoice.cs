using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoShop.BusinessDomain.Orders
{
    public class Invoice
    {
        [BsonElement("payment_method")]
        public string PaymentMethod { get; set; }

        [BsonElement("status")]
        public string Status { get; set; }

        //[BsonElement("paypal_orderid")]
        //public string PayPalOrderId { get; set; }

        //[BsonElement("paypal_payerid")]
        //public string PaypalPayerId { get; set; }
    }
    public static class InvoiceStatus
    {
        public const string Paid = "Paid";
        public const string Pending = "Pending";
        public const string Cancel = "Cancel";
    }

    public static class PaymentMethod
    {
        public const string ShipCod = "Ship COD";
    }
}
