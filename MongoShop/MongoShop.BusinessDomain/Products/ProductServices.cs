using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoShop.BusinessDomain.Categories;
using MongoShop.Infrastructure.Helpers;

namespace MongoShop.BusinessDomain.Products
{
    public class ProductServices : IProductServices
    {
        private readonly IMongoCollection<Product> _collection;
        private readonly string _collectionName;

        public ProductServices(IMongoClient mongoClient, IOptions<DatabaseSetting> settings)
        {
            _collectionName = MongoDbHelper.GetCollectionName(this.GetType().Name);

            var database =
            mongoClient.GetDatabase(settings.Value.DatabaseName);

            _collection = database.GetCollection<Product>(_collectionName);
        }

        /// <inheritdoc/>     
        public async Task<Product> AddAsync(Product product)
        {
            product.Status = true;
            product.CreatedAt = DateTime.Now;
            product.UpdatedAt = product.CreatedAt;

            await _collection.InsertOneAsync(product);
            return product;
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
            product.Id = id;
            product.Status = true;
            product.UpdatedAt = DateTime.Now;

            await _collection.ReplaceOneAsync(c => c.Id == id, product);
        }

        /// <inheritdoc/>  
        public async Task<List<Product>> GetAllAsync()
        {

            var resultOfJoin = _collection.Aggregate()
                .Match(p => p.Status == true)
                .Lookup(foreignCollectionName: "category", localField: "CategoryId", foreignField: "_id", @as: "Category")
                .Unwind("Category")
                .As<Product>();

            return await resultOfJoin.ToListAsync();
        }

        /// <inheritdoc/>  
        public async Task<Product> GetByIdAsync(string id)
        {
            var product = _collection.Aggregate()
                .Match(p => p.Id == id && p.Status == true)
                .Lookup(foreignCollectionName: "category", localField: "CategoryId", foreignField: "_id", @as: "Category")
                .Unwind("Category")
                .As<Product>();

            return await product.SingleOrDefaultAsync();
        }

        /// <inheritdoc/>  
        public async Task<List<Product>> GetByNameAsync(string name)
        {
            var product = _collection.Aggregate()
               .Match(p => p.Name == name && p.Status == true)
               .Lookup(foreignCollectionName: "category", localField: "CategoryId", foreignField: "_id", @as: "Category")
               .Unwind("Category")
               .As<Product>();

            return await product.ToListAsync();
        }
    }
}
