using MongoShop.Areas.Admin.ViewModels.Category;
using MongoShop.Services.FileUpload;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MongoShop.Areas.Admin.ViewModels.Product
{
    public class CreateProductViewModel
    {
        [DisplayName("Product name")]
        public string Name { get; set; }

        public double Price { get; set; }

        [DisplayName("Quantity in stock")]
        public int StockQuantity { get; set; }

        public string Size { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ImagesUpload Images { get; set; }

        public CategoryViewModel Category { get; set; }
    }
}
