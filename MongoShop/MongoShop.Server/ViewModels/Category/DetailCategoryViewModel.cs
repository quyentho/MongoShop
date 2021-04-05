using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MongoShop.Server.ViewModels.Category
{
    public class DetailCategoryViewModel
    {
        public string Id { get; set; }

        [DisplayName("Category name")]
        public string Name { get; set; }
    }
}
