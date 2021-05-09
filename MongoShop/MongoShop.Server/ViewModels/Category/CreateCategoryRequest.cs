using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MongoShop.Server.ViewModels.Category
{
    public class CreateCategoryRequest
    {
        [Required]
        [DisplayName("Category name")]
        public string Name { get; set; }
        [Required]
        public bool IsMainCate { get; set; }
    }
}
