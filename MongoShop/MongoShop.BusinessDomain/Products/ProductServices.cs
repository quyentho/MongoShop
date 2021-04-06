﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
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

            var list = await _collection.FindAsync(c => c.Status == true);
            return await list.ToListAsync();
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
            var products = await _collection.FindAsync(p => p.Name == name && p.Status == true);

            return await products.ToListAsync();
        }
    }
}
