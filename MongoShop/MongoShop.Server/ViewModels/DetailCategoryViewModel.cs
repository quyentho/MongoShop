using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MongoShop.Server.ViewModels
{
    public class DetailCategoryViewModel
    {
        public string Id { get; set; }

        [DisplayName("Category name")]
        public string Name { get; set; }
    }
}
