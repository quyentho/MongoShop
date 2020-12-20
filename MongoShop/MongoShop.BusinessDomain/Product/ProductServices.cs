using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoShop.BusinessDomain.Product
{
    public class ProductServices : IProductServices
    {
        private const string CollectionName = "product";

        private readonly IMongoDatabase _mongoDatabase;

        private readonly IMongoCollection<Product> _collection;

        public ProductServices(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;

            _collection = _mongoDatabase.GetCollection<Product>(CollectionName);
        }
        
        /// <summary>
        /// Add one product to collection.
        /// </summary>
        /// <param name="product">product model.</param>
        /// <returns></returns>
        public async Task AddAsync(Product product)
        {
            await _collection.InsertOneAsync(product);
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task Edit(string id, Product product)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
