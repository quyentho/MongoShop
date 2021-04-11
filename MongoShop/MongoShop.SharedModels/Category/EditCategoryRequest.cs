using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MongoShop.SharedModels.Category
{
    public class EditCategoryRequest
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [DisplayName("Category name")]
        public string Name { get; set; }
    }
}
