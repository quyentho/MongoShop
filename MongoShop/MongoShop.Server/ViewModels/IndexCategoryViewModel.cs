using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MongoShop.Server.ViewModels
{
    public class IndexCategoryViewModel
    {
        public string Id { get; set; }

        [DisplayName("Category name")]
        public string Name { get; set; }
    }
}
