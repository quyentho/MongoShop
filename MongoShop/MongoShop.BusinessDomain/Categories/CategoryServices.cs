﻿using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoShop.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoShop.BusinessDomain.Categories
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IMongoCollection<Category> _collection;
        private readonly string _collectionName;

        public CategoryServices(IMongoClient mongoClient, IOptions<DatabaseSetting> settings)
        {
            _collectionName = MongoDbHelper.GetCollectionName(this.GetType().Name);

            var database =
            mongoClient.GetDatabase(settings.Value.DatabaseName);

            _collection = database.GetCollection<Category>(_collectionName);
        }

        ///<inheritdoc/>
        public async Task<Category> AddAsync(Category category)
        {
            category.Status = true;

            await _collection.InsertOneAsync(category);

            return category;
        }

        ///<inheritdoc/>
        public async Task DeleteAsync(string id)
        {
            var categoryFromDb = await GetByIdAsync(id);
            if (categoryFromDb != null && categoryFromDb.Status == true)
            {
                categoryFromDb.Status = false;
                await _collection.ReplaceOneAsync(c => c.Id == id, categoryFromDb);
            }
        }

        ///<inheritdoc/>
        public async Task EditAsync(string id, Category category)
        {
            var categoryFromDb = await GetByIdAsync(id);
            if (categoryFromDb is null)
            {
                throw new InvalidOperationException("Try to update not exist object");
            }

            category.Id = id;
            category.Status = true;

            await _collection.ReplaceOneAsync(c => c.Id == id, category);
        }

        /// <inheritdoc/>
        public async Task<List<Category>> GetAllMainCategoryAsync()
        {
            var list = await _collection.FindAsync(c => c.Status == true && c.IsMainCate == true);
            return list.ToList().OrderBy(c => c.Name).ToList();
        }

        /// <inheritdoc/>
        public async Task<List<Category>> GetAllAsync()
        {
            var list = await _collection.FindAsync(c => c.Status == true);
            return await list.ToListAsync();
        }
        /// <inheritdoc/>
        public async Task<List<Category>> GetAllSubCategoryAsync()
        {
            var list = await _collection.FindAsync(c => c.Status == true && c.IsMainCate == false);
            return list.ToList().OrderBy(c => c.Name).ToList();
        }

        ///<inheritdoc/>
        public async Task<Category> GetByIdAsync(string id)
        {
            var category = await _collection.FindAsync(c => c.Id == id && c.Status == true);
            return await category.SingleOrDefaultAsync();
        }

        ///<inheritdoc/>
        public async Task<Category> GetByNameAsync(string name)
        {
            var category = await _collection.FindAsync(c => c.Name == name && c.Status == true);
            return await category.SingleOrDefaultAsync();
        }
    }
}
