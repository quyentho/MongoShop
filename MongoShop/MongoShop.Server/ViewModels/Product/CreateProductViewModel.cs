﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MongoShop.Areas.Admin.ViewModels.Product
{
    public class CreateProductViewModel
    {
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

        public List<IFormFile> ImagesUpload { get; set; }

        public string Category { get; set; }
    }
}
