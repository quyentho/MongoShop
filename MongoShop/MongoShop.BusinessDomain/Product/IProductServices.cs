using System.Collections.Generic;

namespace MongoShop.BusinessDomain.Product
{
    public interface IProductServices
    {
        List<Product> GetAll();
        void Add(Product product);
        void Edit(string id, Product product);
        void Delete(string id);
        Product GetById(string id);
        List<Product> GetByName(string name);
    }
}
