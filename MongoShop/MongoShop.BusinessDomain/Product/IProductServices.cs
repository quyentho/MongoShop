using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoShop.BusinessDomain.Product
{
    public interface IProductServices
    {

        void Add(Product product);
        void Edit(string id, Product product);
        void Delete(string id);
    }
}
