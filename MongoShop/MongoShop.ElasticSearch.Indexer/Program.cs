using System;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using MongoDB.Driver;
using MongoShop.BusinessDomain.Categories;
using MongoShop.BusinessDomain.Products;
using Nest;

namespace MongoShop.ElasticSearch.Indexer
{
    class Program
    {
        private static ElasticClient Client { get; set; }
        private static IMongoCollection<Product> Collection { get; set; }


        private static string CurrentIndexName { get; set; }



        static void Main(string[] args)
        {

            Collection = ElasticSearchConfiguration.GetCollection();

            Client = ElasticSearchConfiguration.GetClient();
            CurrentIndexName = ElasticSearchConfiguration.CreateIndexName();

            CreateIndex();
            IndexDumps();
            SwapAlias();

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static void CreateIndex()
        {
            Client.Indices.Create(CurrentIndexName, i => i
                .Settings(s => s
                    .NumberOfShards(2)
                    .NumberOfReplicas(0)
                    .Setting("index.mapping.nested_objects.limit", 12000)
                    .Analysis(Analysis)
                )
                .Map<Product>(MapPackage)
            );
        }

        private static TypeMappingDescriptor<Product> MapPackage(TypeMappingDescriptor<Product> map) => map
            .AutoMap()
            .Properties(ps => ps
                .Text(t => t
                    .Name(p => p.Name)
                    .Analyzer("product-name-analyzer")
                    .Fields(f => f
                        .Text(p => p
                            .Name("keyword")
                            .Analyzer("product-name-keyword")
                        )
                        .Text(p => p
                            .Name("normalize")
                            .Analyzer("product-name-ascii-analyzer"))
                        .Keyword(p => p
                            .Name("raw")
                        )
                    )
                )

                .Nested<Category>(n => n
                    .Name(p => p.Category)
                    .AutoMap()
                )
                .Nested<Category>(n => n
                    .Name(p => p.SubCategory)
                    .AutoMap()
                )
                );

        private static AnalysisDescriptor Analysis(AnalysisDescriptor analysis) => analysis
            .TokenFilters(tokenfilters => tokenfilters
                .WordDelimiter("product-name-token-filter", w => w
                    .PreserveOriginal()
                    .SplitOnNumerics()
                    .GenerateNumberParts(false)
                    .GenerateWordParts()
                )
            )
            .Analyzers(analyzers => analyzers
                .Custom("product-name-analyzer", c => c
                    .Tokenizer("standard")
                    .Filters("product-name-token-filter", "lowercase")
                )
                .Custom("product-name-ascii-analyzer", c => c
                     .Tokenizer("standard")
                     .Filters("product-name-token-filter", "asciifolding", "lowercase")
                )
                .Custom("product-name-keyword", c => c
                    .Tokenizer("keyword")
                    .Filters("lowercase")
                )
            );

        static void IndexDumps()
        {
            Console.WriteLine("Setting up a lazy xml files reader that yields packages...");
            var products = Collection.Find(_ => true).ToList();

            Console.WriteLine(products[0].Name);

            Console.Write("Indexing documents into Elasticsearch...");
            var waitHandle = new CountdownEvent(1);

            var bulkAll = Client.BulkAll(products, b => b
                .Index(CurrentIndexName)
                .BackOffRetries(2)
                .BackOffTime("30s")
                //.RefreshOnCompleted(true)
                .MaxDegreeOfParallelism(4)
                .Size(1000)
            );

            ExceptionDispatchInfo captureInfo = null;

            bulkAll.Subscribe(new BulkAllObserver(
                onNext: b => Console.Write("."),
                onError: e =>
                {
                    captureInfo = ExceptionDispatchInfo.Capture(e);
                    waitHandle.Signal();
                },
                onCompleted: () => waitHandle.Signal()
            ));

            waitHandle.Wait(TimeSpan.FromMinutes(30));
            captureInfo?.Throw();
            Console.WriteLine("Done.");
        }

        private static void SwapAlias()
        {
            var indexExists = Client.Indices.Exists(ElasticSearchConfiguration.LiveIndexAlias).Exists;

            Client.Indices.BulkAlias(aliases =>
            {
                if (indexExists)
                    aliases.Add(a => a
                        .Alias(ElasticSearchConfiguration.OldIndexAlias)
                        .Index(Client.GetIndicesPointingToAlias(ElasticSearchConfiguration.LiveIndexAlias).First())
                    );

                return aliases
                    .Remove(a => a.Alias(ElasticSearchConfiguration.LiveIndexAlias).Index("*"))
                    .Add(a => a.Alias(ElasticSearchConfiguration.LiveIndexAlias).Index(CurrentIndexName));
            });

            var oldIndices = Client.GetIndicesPointingToAlias(ElasticSearchConfiguration.OldIndexAlias)
                .OrderByDescending(name => name)
                .Skip(2);

            foreach (var oldIndex in oldIndices)
                Client.Indices.Delete(oldIndex);
        }
    }
}
