//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Bogus;
//using MongoDB.Bson;
//using MongoDB.Driver;
//using MongoShop.BusinessDomain;
//using MongoShop.BusinessDomain.Categories;
//using MongoShop.BusinessDomain.Products;

//namespace MongoShop.DataInitialize
//{
//    public class DataInitializer
//    //{
//    //    private readonly IProductServices _productServices;
//    //    private readonly ICategoryServices _categoryServices;

//    //    private IMongoCollection<Category> _collection;
//    //    private readonly IDatabaseSetting _databaseSetting;
//    //    private const string _categoryCollection = "category";
//    //    IMongoDatabase _database;
//    //    public DataInitializer(IDatabaseSetting databaseSetting, IProductServices productServices, ICategoryServices categoryServices)
//    //    {
//    //        _databaseSetting = databaseSetting;
//    //        var client = new MongoClient(_databaseSetting.ConnectionString);
//    //        _database = client.GetDatabase(_databaseSetting.DatabaseName);


//    //        this._productServices = productServices;
//    //        this._categoryServices = categoryServices;
//    //    }

//    //    public void Generate()
//    //    {
//    //        Faker<Category> faker = new Faker<Category>()
//    //       .RuleFor(p => p.Id, f => ObjectId.GenerateNewId().ToString())
//    //       .RuleFor(p => p.Name, f => f.Lorem.Word())
//    //       .RuleFor(p => p.Status, f => true);


//    //        _collection = _database.GetCollection<Category>(_categoryCollection);

//    //        static string[] sizes = new string[] { "XS", "S", "M", "L", "XL" };
//    //        Faker<Product> faker = new Faker<Product>()
//    //            .RuleFor(p => p.Id, f => ObjectId.GenerateNewId().ToString())
//    //            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
//    //            .RuleFor(p => p.Price, f => f.Random.Int(200_000, 950_000))
//    //            .RuleFor(p => p.StockQuantity, f => f.Random.Int(10, 40))
//    //            .RuleFor(p => p.Size, f => f.PickRandom(sizes))
//    //            .RuleFor(p => p.Status, f => true)
//    //            .RuleFor(p => p.CreatedAt, f => f.Date.Between(new DateTime(2019, 1, 1), new DateTime(2020, 12, 31)))
//    //            .RuleFor(p => p.UpdatedAt, (f, p) => f.Date.Future(1, p.CreatedAt))
//    //            .RuleFor(p => p.CategoryId, f.)
 

//    //    }




//    }
//}
