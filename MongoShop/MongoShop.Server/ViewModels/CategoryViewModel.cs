using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MongoShop.Server.ViewModels
{
    public class CategoryViewModel
    {
        public string Id { get; set; }

        [DisplayName("Category name")]
        public string Name { get; set; }
    }
}
