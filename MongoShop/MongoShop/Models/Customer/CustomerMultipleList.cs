using System.Collections.Generic;

namespace MongoShop.Models.Customer
{
    public class CustomerMultipleList
    {
        public IEnumerable<IndexViewModel> ShirtCollection { get; set; }
        public IEnumerable<IndexViewModel> TrouserCollection { get; set; }
        public IEnumerable<IndexViewModel> AccessoriesCollection { get; set; }
    }
}
