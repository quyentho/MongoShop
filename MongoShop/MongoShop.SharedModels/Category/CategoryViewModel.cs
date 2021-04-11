using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MongoShop.SharedModels.Category
{
    public class CategoryViewModel
    {
        [Required]
        public string Id { get; set; }

        [DisplayName("Category name")]
        public string Name { get; set; }
    }
}
