using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MongoShop.Areas.Admin.ViewModels.Category
{
    public class IndexCategoryViewModel
    {
        [Required]
        public string Id { get; set; }

        //[Required]
        [DisplayName("Category name")]
        public string Name { get; set; }
    }
}
