using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MongoShop.BusinessDomain.Categories
{
    public interface ICategoryServices
    {

        /// <summary>
        /// Gets all active categories from database.
        /// </summary>
        /// <returns>List of categories.</returns>
        Task<List<Category>> GetAllAsync();

        /// <summary>
        /// Gets all active main categories from database.
        /// </summary>
        /// <returns>List of categories.</returns>
        Task<List<Category>> GetAllMainCategoryAsync();

        /// <summary>
        /// Gets all active sub categories from database.
        /// </summary>
        /// <returns>List of categories.</returns>
        Task<List<Category>> GetAllSubCategoryAsync();

        /// <summary>
        /// Adds new category.
        /// </summary>
        /// <param name="category">New Category.</param>
        /// <returns>Newly created category.</returns>
        Task<Category> AddAsync(Category category);

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

        /// <summary>
        /// Gets active category by Id.
        /// </summary>
        /// <param name="id">Category id.</param>
        /// <returns>Category if found, null if not found.</returns>
        Task<Category> GetByIdAsync(string id);
    }
}
