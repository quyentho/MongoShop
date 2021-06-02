using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoShop.BusinessDomain.Categories;
using MongoShop.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoShop.BusinessDomain.Products
{
    public class HomePageProductServices : IHomePageProductServices
    {
        private readonly IMongoCollection<Product> _collection;
        private readonly string _collectionName;

        public HomePageProductServices(IMongoClient mongoClient, IOptions<DatabaseSetting> settings)
        {
            _collectionName = MongoDbHelper.GetCollectionName(this.GetType().Name);

            var database =
            mongoClient.GetDatabase(settings.Value.DatabaseName);

            _collection = database.GetCollection<Product>(_collectionName);
        }

        /// <inheritdoc/>     
        public async Task<List<Product>> AddAsync(List<Product> product)
        {
            var filter = Builders<Product>.Filter.Empty;
            
            await _collection.DeleteManyAsync(filter);
            await _collection.InsertManyAsync(product);
            return product;
        }


        public Task DeleteAsync(string id, Product product)
        {
            throw new NotImplementedException();
        }

        public Task EditAsync(string id, Product product)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Product>> GetByMainCategoryAsync(Category mainCategory)
        {
            var list = await _collection.FindAsync(c => c.Status == true && c.Category == mainCategory);
            return await list.ToListAsync();
        }

        public Task<List<Product>> GetByNameAsync(string productName)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetBySubCategoryAsync(Category subCategory)
        {
            throw new NotImplementedException();
        }
    }
}
