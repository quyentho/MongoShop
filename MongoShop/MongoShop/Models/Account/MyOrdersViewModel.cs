using System;
using System.Collections.Generic;

namespace MongoShop.Models.Account
{
    public class MyOrdersViewModel
    {
        public string OrderId { get; set; }

        public List<ProductViewModel> OrderedProducts { get; set; }

        public string InvoiceStatus { get; set; }

        public DateTime CreatedDate { get; set; }
    }

    public class ProductViewModel
    {
        public int Quantity { get; set; }
        public double TotalPrice 
        { get => Quantity * Price;}

        public string ProductName { get; set; }


        public double Price { get; set; }

    }
}
