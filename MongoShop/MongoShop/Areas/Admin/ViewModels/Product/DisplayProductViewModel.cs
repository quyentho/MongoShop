using System;
using System.Collections.Generic;
using System.ComponentModel;
using MongoShop.Areas.Admin.ViewModels.Category;

namespace MongoShop.Areas.Admin.ViewModels.Product
{
    public class DisplayProductViewModel
    {
        public string Id { get; set; }

        [DisplayName("Product name")]

        public string Name { get; set; }

        public double Price { get; set; }

        [DisplayName("Quantity in stock")]
        public int StockQuantity { get; set; }

        public string Size { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<string> Images { get; set; }

        public CategoryViewModel Category { get; set; }
    }
}
