using MongoShop.Areas.Admin.ViewModels.Category;
using MongoShop.Services.FileUpload;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MongoShop.Areas.Admin.ViewModels.Product
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {
            Category = new CategoryViewModel();
            Images = new ImagesUpload();
        }

        public string Id { get; set; }

        [DisplayName("Product name")]
        [Required]
        public string Name { get; set; }

        [Range(0, 1_000_000_000)]
        public double Price { get; set; }

        [DisplayName("Quantity in stock")]
        [Range(0, 1_000_000_000)]
        public int StockQuantity { get; set; }

        [Required]
        public string Size { get; set; }

        public ImagesUpload Images { get; set; }

        public CategoryViewModel Category { get; set; }
    }
}
