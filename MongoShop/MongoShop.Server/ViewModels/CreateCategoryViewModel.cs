using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MongoShop.Server.ViewModels
{
    public class CreateCategoryViewModel
    {
        [Required]
        [DisplayName("Category name")]
        public string Name { get; set; }
    }
}
