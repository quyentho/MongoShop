using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MongoShop.SharedModels.Category
{
    public class CreateCategoryRequest
    {
        [Required]
        [DisplayName("Category name")]
        public string Name { get; set; }
    }
}
