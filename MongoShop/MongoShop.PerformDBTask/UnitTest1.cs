using MongoDB.Bson;
using MongoDB.Driver;
using MongoShop.BusinessDomain.Categories;
using MongoShop.BusinessDomain.Products;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace MongoShop.PerformDBTask
{
    
    public class Tests
    {
        private IMongoCollection<Product> _productCollection;
        private IMongoCollection<Category> _categoryCollection;


        [SetUp]
        public void SetUp()
        {
            MongoClient dbClient = new MongoClient("mongodb://localhost:27017");
            var database =
                dbClient.GetDatabase("MongoShop");
            _productCollection = database.GetCollection<Product>("product");
            _categoryCollection = database.GetCollection<Category>("category");

        }
        [Test]
        public void Test1()
        {
            
            JArray obj = new JArray();
            using (StreamReader file = File.OpenText(@"C:\Users\QuyenTho\projects\clothes\owen_dataset.json"))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                obj = (JArray)JToken.ReadFrom(reader);
            }

            Product product = new Product();
            Random r = new Random();
            var list = new List<string> { "S", "M", "L", "XL", "XXL" };

            Category QuanAu = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.QuanAu,
                Status = true
            };
            Category Kaki = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.QuanKaki,
                Status = true
            };
            Category Short = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.QuanShort,
                Status = true
            };
            Category Jeans = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.QuanJeans,
                Status = true
            };
            Category Jogger = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.QuanJogger,
                Status = true
            };
            Category QuanTay = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.QuanTay,
                Status = true
            };
            Category DayLung = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.DayLung,
                Status = true
            };
            Category Vi = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.Vi,
                Status = true
            };
            Category PhuKien = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.PhuKien,
                Status = true
            };

            Category AoThun = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.AoThun,
                Status = true
            };
            Category Polo = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.Polo,
                Status = true
            };
            Category Tshirt = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.Tshirt,
                Status = true
            };
            Category Veston = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.Veston,
                Status = true
            };
            Category Jacket = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.Jacket,
                Status = true
            };
            Category AoLen = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.AoLen,
                Status = true
            };
            Category Blazor = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Blazor",
                Status = true
            };
            Category SoMi = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Sơ Mi",
                Status = true
            };
            _categoryCollection.InsertMany(new List<Category>(){ QuanAu, Kaki, Short, Jeans, Jogger, QuanTay, DayLung, Vi, PhuKien, AoThun, Polo, Tshirt,Veston,Jacket,AoLen,Blazor,SoMi});

            foreach (JObject item in obj)
            {
                product.Id = ObjectId.GenerateNewId().ToString();
                product.Name = item.GetValue("name").ToString();
                product.Price = double.Parse(item.GetValue("price").ToString().Replace(".", ","), CultureInfo.InvariantCulture);
                product.Images.Add(item.SelectToken("images[0].path").ToString());
                if (item.GetValue("name").ToString().Contains(ClothesCategory.SoMi,StringComparison.OrdinalIgnoreCase))
                {
                    product.Category = SoMi;
                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.Blazor, StringComparison.OrdinalIgnoreCase))
                {
                    product.Category = Blazor;
                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.AoLen, StringComparison.OrdinalIgnoreCase))
                {
                    product.Category = AoLen;
                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.Jacket, StringComparison.OrdinalIgnoreCase))
                {
                    product.Category = Jacket;
                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.Veston, StringComparison.OrdinalIgnoreCase))
                {
                    product.Category = Veston;
                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.Tshirt, StringComparison.OrdinalIgnoreCase))
                {
                    product.Category = Tshirt;
                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.Polo, StringComparison.OrdinalIgnoreCase))
                {
                    product.Category = Polo;
                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.AoThun, StringComparison.OrdinalIgnoreCase))
                {
                    product.Category = AoThun;
                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.QuanAu, StringComparison.OrdinalIgnoreCase))
                {
                    product.Category = QuanAu;
                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.QuanKaki, StringComparison.OrdinalIgnoreCase))
                {
                    product.Category = Kaki;
                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.QuanShort, StringComparison.OrdinalIgnoreCase))
                {
                    product.Category = Short;
                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.QuanJeans, StringComparison.OrdinalIgnoreCase))
                {
                    product.Category = Jeans;
                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.QuanJogger, StringComparison.OrdinalIgnoreCase))
                {
                    product.Category = Jogger;
                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.QuanTay, StringComparison.OrdinalIgnoreCase))
                {
                    product.Category = QuanTay;
                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.DayLung, StringComparison.OrdinalIgnoreCase))
                {
                    product.Category = DayLung;
                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.Vi, StringComparison.OrdinalIgnoreCase))
                {
                    product.Category = Vi;
                }
                else 
                {
                    product.Category = PhuKien;
                }
                product.CreatedAt = DateTime.Now;
                product.Status = true;
                product.StockQuantity = r.Next(20, 100);
                product.UpdatedAt = DateTime.Now;
                
                var random = new Random();
                int index = random.Next(list.Count);
                product.Size = list[index];
                TestContext.WriteLine(product);
                _productCollection.InsertOne(product);

            }
        }

        [Test]
        public void Test2()
        {
            JArray obj = new JArray();
            using (StreamReader file = File.OpenText(@"C:\Users\QuyenTho\projects\clothes\owen_dataset.json"))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                obj = (JArray)JToken.ReadFrom(reader);
            }
            foreach (JObject item in obj)
            {
                TestContext.WriteLine(double.Parse(item.GetValue("price").ToString().Replace(".", ","), CultureInfo.InvariantCulture));
            }
            //var result = Double.Parse("350.000".Replace(".",","), CultureInfo.InvariantCulture);
            //TestContext.WriteLine(result);
        }

    }
    public static class ClothesCategory
    {
        public const string SoMi = "Áo sơ mi";
        public const string Blazor = "Blazor";
        public const string AoLen = "Áo len";
        public const string Jacket = "Jacket";
        public const string Veston = "Veston";
        public const string Tshirt = "Tshirt";
        public const string Polo = "Polo";
        public const string AoThun = "Áo Thun";
        public const string QuanAu = "Quần Âu";
        public const string QuanKaki = "Quần Kaki";
        public const string QuanShort = "Quần Short";
        public const string QuanJeans = "Quần Jeans";
        public const string QuanJogger = "Quần Jogger";
        public const string QuanTay = "Quần Tây";
        public const string DayLung = "Dây Lưng";
        public const string Vi = "Ví";
        public const string PhuKien = "Phụ Kiện";
    }
}