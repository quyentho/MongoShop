using System.ComponentModel;

namespace MongoShop.Models.Admin
{
    public class CategoryViewModel
    {
        public string Id { get; set; }

        [DisplayName("Category name")]
        public string Name { get; set; }
    }
}
