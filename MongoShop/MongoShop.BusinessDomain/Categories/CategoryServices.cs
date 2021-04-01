using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoShop.BusinessDomain.Categories
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IMongoCollection<Category> _collection;
        private const string CollectionName = "category";

        public CategoryServices(IMongoClient mongoClient, IOptions<DatabaseSetting> settings)
        {
            var database =
            mongoClient.GetDatabase(settings.Value.DatabaseName);

            _collection = database.GetCollection<Category>(CollectionName);
        }

        public async Task AddAsync(Category category)
        {
            category.Status = true;

            await _collection.InsertOneAsync(category);
        }

        public async Task DeleteAsync(string id, Category category)
        {
            category.Status = false;
            await _collection.ReplaceOneAsync(c => c.Id == id, category);
        }

        public async Task EditAsync(string id, Category category)
        {
            category.Status = true;
            await _collection.ReplaceOneAsync(c => c.Id == id, category);
        }

        public async Task<List<Category>> GetAllAsync()
        {
            var list = await _collection.FindAsync(c => c.Status == true);
            return await list.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(string id)
        {
            var category = await _collection.FindAsync(c => c.Id == id && c.Status == true);
            return await category.SingleOrDefaultAsync();
        }
    }
}
