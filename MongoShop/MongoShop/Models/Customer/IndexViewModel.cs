﻿using System.Collections.Generic;
using System.ComponentModel;

namespace MongoShop.Models.Customer
{
    public class IndexViewModel
    {
        public string Id { get; set; }

        [DisplayName("Product name")]
        public string Name { get; set; }

        public double Price { get; set; }

        [DisplayName("Quantity in stock")]
        public int StockQuantity { get; set; }

        public string Size { get; set; }

        public string Category { get; set; }

        public List<string> Images { get; set; }
    }
}
