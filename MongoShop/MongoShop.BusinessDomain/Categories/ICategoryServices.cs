using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MongoShop.BusinessDomain.Categories
{
    public interface ICategoryServices
    {
        /// <summary>
        /// Gets all categories from database.
        /// </summary>
        /// <returns>List of categories.</returns>
        Task<List<Category>> GetAllAsync();

        /// <summary>
        /// Adds new category.
        /// </summary>
        /// <param name="category">New Category.</param>
        /// <returns>No return.</returns>
        Task AddAsync(Category category);

        /// <summary>
        /// Edit existing category.Throw <exception cref="InvalidOperationException"> if category id not found.
        /// </summary>
        /// <param name="id">Category id.</param>
        /// <param name="category">Edited category.</param>
        /// <returns>No return.</returns>
        Task EditAsync(string id, Category category);

        /// <summary>
        /// Soft delete existing category by changing status.
        /// </summary>
        /// <param name="id">Category id.</param>
        /// <returns>Nothing.</returns>
        Task DeleteAsync(string id);
        Task<Category> GetByIdAsync(string id);
    }
}
