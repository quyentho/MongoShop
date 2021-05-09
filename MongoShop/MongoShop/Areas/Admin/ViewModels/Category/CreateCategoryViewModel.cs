using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MongoShop.Areas.Admin.ViewModels.Category
{
    public class CreateCategoryViewModel
    {
        [Required]
        [DisplayName("Category name")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Is main category")]
        public bool IsMainCate { get; set; } = false;
    }
}
