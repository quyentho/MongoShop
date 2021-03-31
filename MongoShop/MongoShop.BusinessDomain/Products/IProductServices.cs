using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoShop.BusinessDomain.Products
{
    public interface IProductServices
    {
        /// <summary>
        /// Get all products with status = true
        /// </summary>
        /// <returns></returns>
        Task<List<Product>> GetAllAsync();
        
        /// <summary>
        /// edit a product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        Task EditAsync(string id, Product product);

        /// <summary>
        /// Delete the selected product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        Task DeleteAsync(string id, Product product);

        /// <summary>
        /// Get all products by ID with status = true
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Product> GetByIdAsync(string id);

        /// <summary>
        /// Get all Product by Name with status = true
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<List<Product>> GetByNameAsync(string name);


        /// <summary>
        /// Add a new product
        /// </summary>
        /// <param name="product">product to add.</param>
        /// <returns></returns>
        Task AddAsync(Product product);
    }
}
