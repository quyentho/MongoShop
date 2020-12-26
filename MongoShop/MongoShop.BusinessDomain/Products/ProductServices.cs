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

        /// <inheritdoc/>     
        public async Task AddAsync(Product product)
        {
            product.Status = true;
            await _collection.InsertOneAsync(product);
        }

        /// <inheritdoc/>  
        public async Task DeleteAsync(string id, Product product)
        {
            product.Status = false;
            await _collection.ReplaceOneAsync(c => c.Id == id, product);
        }

        /// <inheritdoc/>  
        public async Task EditAsync(string id, Product product)
        {
            await _collection.ReplaceOneAsync(c => c.Id == id, product);
        }

        /// <inheritdoc/>  
        public async Task<List<Product>> GetAllAsync()
        {
            var _list = await _collection.FindAsync(c => c.Status == true);
            //var debug = await _list.ToListAsync();
            return await _list.ToListAsync();

        }

        /// <inheritdoc/>  
        public async Task<Product> GetByIdAsync(string id)
        {
            var _product = await _collection.FindAsync(c => c.Id == id && c.Status == true);
            //var debug = await _product.SingleOrDefaultAsync();
            return await _product.SingleOrDefaultAsync();
        }

        /// <inheritdoc/>  
        public async Task<List<Product>> GetByNameAsync(string name)
        {
            var _list = await _collection.FindAsync(c => c.Name == name && c.Status == true);
            //var debug = await _list.ToListAsync();
            return await _list.ToListAsync();
        }
    }
}
