using System.ComponentModel;

namespace MongoShop.Areas.Admin.ViewModels.Category
{
    public class CategoryViewModel
    {
        public string Id { get; set; }

        [DisplayName("Category name")]
        public string Name { get; set; }
    }
}
