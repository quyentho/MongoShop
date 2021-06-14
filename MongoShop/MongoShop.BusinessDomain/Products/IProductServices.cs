using MongoShop.BusinessDomain.Categories;
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
        /// Get all products by main category
        /// </summary>
        /// <returns></returns>
        Task<List<Product>> GetByMainCategoryAsync(Category mainCategory);

        /// <summary>
        /// Get all products by sub category
        /// </summary>
        /// <returns></returns>
        Task<List<Product>> GetBySubCategoryAsync(Category subCategory);

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
        /// Get product by ID with status = true
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Product> GetByIdAsync(string id);

        /// <summary>
        /// Get all products contain the name with status = true
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<List<Product>> GetByNameAsync(string productName);


        /// <summary>
        /// Add a new product
        /// </summary>
        /// <param name="product">product to add.</param>
        /// <returns>Created Product.</returns>
        Task<Product> AddAsync(Product product);

        /// <summary>
        /// Get products by Image
        /// </summary>
        /// <param name="image">product to add.</param>
        /// <returns>Created Product.</returns>
        Task<List<Product>> GetByImageAsync(string imgPath);
    }
}
