using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoShop.Models.Customer
{
    public class CustomerMultipleList
    {
        public IEnumerable<IndexViewModel> Collection1 { get; set; }
        public IEnumerable<IndexViewModel> Collection2 { get; set; }
    }
}
