using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoShop.BusinessDomain.Products
{
    public class ProductServices : IProductServices
    {
        private readonly IMongoCollection<Product> _collection;
        private readonly IDatabaseSetting _databaseSetting;
        private const string CollectionName = "product";

        public ProductServices(IDatabaseSetting databaseSetting)
        {

            _databaseSetting = databaseSetting;

            var client = new MongoClient(_databaseSetting.ConnectionString);
            var database = client.GetDatabase(_databaseSetting.DatabaseName);

            _collection = database.GetCollection<Product>(CollectionName);
        }

        /// <summary>
        /// Add one product to collection.
        /// </summary>
        /// <param name="product">product model.</param>
        /// <returns></returns>
        public async Task AddAsync(Product product)
        {
            product.Status = true;
            await _collection.InsertOneAsync(product);
        }

        public Task Delete(string id)
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

        public Task<List<Product>> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}
