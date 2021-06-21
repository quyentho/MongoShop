using System.ComponentModel.DataAnnotations;

namespace MongoShop.Server.ViewModels.Order
{
    public class CreateOrderedProductRequest
    {
        [Required]
        public string ProductId { get; set; }

        [Range(1,100)]
        public int OrderedQuantity { get; set; }
    }
}
