using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MongoShop.Server.ViewModels.Category
{
    public class CategoryViewModel
    {
        [Required]
        public string Id { get; set; }

        [DisplayName("Category name")]
        public string Name { get; set; }
    }
}
