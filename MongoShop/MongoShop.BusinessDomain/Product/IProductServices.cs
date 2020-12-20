using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoShop.BusinessDomain.Product
{
    public interface IProductServices
    {
        Task<List<Product>> GetAllAsync();
        
        /// <summary>
        /// add comment.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        Task EditAsync(string id, Product product);
        
        Task Delete(string id);
        
        Task<Product> GetByIdAsync(string id);

        Task<List<Product>> GetByNameAsync(string name);

        Task AddAsync(Product product);
    }
}
