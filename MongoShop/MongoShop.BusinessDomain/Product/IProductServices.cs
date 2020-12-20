using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoShop.BusinessDomain.Product
{
    public interface IProductServices
    {
        List<Product> GetAll();
        
        Task Edit(string id, Product product);
        
        Task Delete(string id);
        
        Task<Product> GetById(string id);

        Task<List<Product>> GetByName(string name);

        Task AddAsync(Product product);
    }
}
