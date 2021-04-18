using MongoShop.SharedModels.Category;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MongoShop.SharedModels.Product
{
    public class ProductViewModel
    {
        [Required]
        public string Id { get; set; }

        [DisplayName("Product name")]
        [Required]
        public string Name { get; set; }

        public double Price { get; set; }

        [DisplayName("Quantity in stock")]
        public int StockQuantity { get; set; }

        public string Size { get; set; }

        public CategoryViewModel Category { get; set; }

        //public List<string> Images { get; set; }

    }
}
