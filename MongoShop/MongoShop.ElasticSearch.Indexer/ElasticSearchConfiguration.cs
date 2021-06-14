using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using MongoDB.Driver;
using MongoShop.BusinessDomain.Products;
using Nest;

namespace MongoShop.ElasticSearch.Indexer
{
    public static class ElasticSearchConfiguration
    {
        public static ElasticClient GetClient() => new ElasticClient(_connectionSettings);
        public static IMongoCollection<Product> GetCollection()
        {
            var MongoClient = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase database = MongoClient.GetDatabase("MongoShop");

            return database.GetCollection<Product>("product");
        }

        static ElasticSearchConfiguration()
        {
            _connectionSettings = new ConnectionSettings(CreateUri(9200))
                .DefaultIndex("mongoshop")
                .DefaultMappingFor<Product>(i => i
                    .IndexName("mongoshop")
                );
        }
        private static readonly ConnectionSettings _connectionSettings;

        public static string LiveIndexAlias => "mongoshop";

        public static string OldIndexAlias => "mongoshop-old";

        public static Uri CreateUri(int port)
        {
            var host = "localhost";

            return new Uri($"http://{host}:{port}");
        }

        public static string CreateIndexName() => $"{LiveIndexAlias}-{DateTime.UtcNow:dd-MM-yyyy-HH-mm-ss}";
    }
}
