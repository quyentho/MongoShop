using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MongoShop.Areas.Admin.ViewModels.Category
{
    public class CategoryViewModel
    {
        [Required]
        public string Id { get; set; }

        [DisplayName("Category name")]
        public string Name { get; set; }
    }
}
