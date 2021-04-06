using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MongoShop.Server.ViewModels.Category
{
    public class EditCategoryViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [DisplayName("Category name")]
        public string Name { get; set; }
    }
}
