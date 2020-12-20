using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public void Add(Product product)
        {
            _collection.InsertOneAsync(product);
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public void Edit(string id, Product product)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAll()
        {
            throw new NotImplementedException();
        }

        public Product GetById(string id)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
