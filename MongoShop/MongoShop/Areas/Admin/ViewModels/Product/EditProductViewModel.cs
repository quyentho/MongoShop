using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MongoShop.Areas.Admin.ViewModels.Product
{
    public class EditProductViewModel
    {
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

        public List<IFormFile> ImagesUpload { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string SelectedCategoryId { get; set; }

        [DisplayName("Category")]
        public List<SelectListItem>? CategoryList { get; set; }

    }
}
