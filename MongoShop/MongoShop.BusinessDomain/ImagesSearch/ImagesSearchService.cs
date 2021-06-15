using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoShop.BusinessDomain.Categories;
using MongoShop.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoShop.BusinessDomain.ImagesSearch
{
    public class ImagesSearchService : IImagesSearchService
    {

        private readonly IMongoCollection<UploadedImage> _collection;
        private readonly string _collectionName;

        public ImagesSearchService(IMongoClient mongoClient, IOptions<DatabaseSetting> settings)
        {
            _collectionName = MongoDbHelper.GetCollectionName(this.GetType().Name);

            var database =
            mongoClient.GetDatabase(settings.Value.DatabaseName);

            _collection = database.GetCollection<UploadedImage>(_collectionName);
        }
        public async Task<UploadedImage> AddAsync(UploadedImage image)
        {
            await _collection.InsertOneAsync(image);

            return image;
        }
    }
}
