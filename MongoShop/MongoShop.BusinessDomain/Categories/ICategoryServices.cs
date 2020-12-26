using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MongoShop.BusinessDomain.Categories
{
    public interface ICategoryServices
    {
        Task<List<Category>> GetAllAsync();

        Task AddAsync(Category category);

        Task EditAsync(string id, Category category);

        Task DeleteAsync(string id, Category category);
    }
}
