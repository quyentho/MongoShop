using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoShop.BusinessDomain.Categories;

namespace MongoShop.BusinessDomain.Products
{
    public class ProductServices : IProductServices
    {
        private readonly IMongoCollection<Product> _collection;
        private readonly IDatabaseSetting _databaseSetting;
        private readonly ICategoryServices _categoryServices;
        private const string CollectionName = "product";

        public ProductServices(IDatabaseSetting databaseSetting, ICategoryServices categoryServices)
        {
            _databaseSetting = databaseSetting;
            this._categoryServices = categoryServices;
            var client = new MongoClient(_databaseSetting.ConnectionString);
            var database = client.GetDatabase(_databaseSetting.DatabaseName);

            _collection = database.GetCollection<Product>(CollectionName);
        }

        /// <inheritdoc/>     
        public async Task AddAsync(Product product, string categoryId)
        {
            product.Status = true;
            product.CreatedAt = DateTime.Now;
            product.UpdatedAt = product.CreatedAt;
            product.CategoryId = categoryId;

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
            var product = await _collection.FindAsync(c => c.Id == id && c.Status == true);
            return await product.SingleOrDefaultAsync();
        }

        /// <inheritdoc/>  
        public async Task<List<Product>> GetByNameAsync(string name)
        {
            var list = await _collection.FindAsync(c => c.Name == name && c.Status == true);
            return await list.ToListAsync();
        }
    }
}
