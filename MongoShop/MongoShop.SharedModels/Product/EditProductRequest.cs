using Microsoft.AspNetCore.Http;
using MongoShop.SharedModels.Category;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MongoShop.SharedModels.Product
{
    public class EditProductRequest
    {
        [Required]
        public string Id { get; set; }

        [DisplayName("Product name")]
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, 1_000_000_000)]
        public double Price { get; set; }

        [Required]
        [DisplayName("Quantity in stock")]
        [Range(0, 1_000_000_000)]
        public int StockQuantity { get; set; }

        [Required]
        public string Size { get; set; }

        public List<string> OldImagePaths { get; set; }

        public List<IFormFile> ImagesUpload { get; set; }

        public CategoryViewModel Category { get; set; }

    }
}
