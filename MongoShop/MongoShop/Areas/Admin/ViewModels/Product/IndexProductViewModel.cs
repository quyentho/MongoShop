using MongoShop.Areas.Admin.ViewModels.Category;
using MongoShop.Services.FileUpload;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MongoShop.Areas.Admin.ViewModels.Product
{
    public class IndexProductViewModel
    {
        public string Id { get; set; }

        [DisplayName("Product name")]
        public string Name { get; set; }

        public double Price { get; set; }

        [DisplayName("Quantity in stock")]
        public int StockQuantity { get; set; }

        public string Size { get; set; }

        public string Category { get; set; }
    }
}
