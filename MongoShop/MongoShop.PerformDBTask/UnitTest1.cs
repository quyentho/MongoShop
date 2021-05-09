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

            JArray owen_obj = new JArray();
            using (StreamReader file = File.OpenText(@"C:\Users\QuyenTho\projects\clothes\owen_dataset.json"))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                owen_obj = (JArray)JToken.ReadFrom(reader);
            }

            
            Random r = new Random();
            var list = new List<string> { "S", "M", "L", "XL", "XXL" };
            Category Quan = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.Quan,
                IsMainCate = true,
                Status = true
            };
            Category Ao = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.Ao,
                IsMainCate = true,
                Status = true
            };
            Category QuanAu = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.QuanAu,
                IsMainCate = false,
                Status = true
            };
            Category Kaki = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.QuanKaki,
                IsMainCate = false,
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
                IsMainCate = false,
                Status = true
            };
            Category Jogger = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.QuanJogger,
                IsMainCate = false,
                Status = true
            };
            Category QuanTay = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.QuanTay,
                Status = true,
                IsMainCate = false,

            };
            Category DayLung = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.DayLung,
                Status = true,
                IsMainCate = false,

            };
            Category Vi = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.Vi,
                Status = true,
                IsMainCate = false,

            };
            Category Tat = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.Tat,
                Status = true,
                IsMainCate = false,

            };
            Category PhuKien = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.PhuKien,
                Status = true,
                IsMainCate = true,

            };

            Category AoThun = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.AoThun,
                Status = true,
                IsMainCate = false,

            };
            Category Polo = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.Polo,
                Status = true,
                IsMainCate = false,

            };
            Category Veston = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.Veston,
                Status = true,
                IsMainCate = false,

            };
            Category Jacket = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.Jacket,
                Status = true,
                IsMainCate = false,
            };
            Category AoLen = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = ClothesCategory.AoLen,
                Status = true,
                IsMainCate = false,

            };
            Category Blazor = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Blazor",
                Status = true,
                IsMainCate = false,

            };
            Category SoMi = new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Sơ Mi",
                Status = true,
                IsMainCate = false,

            };
            _categoryCollection.InsertMany(new List<Category>() { QuanAu, Kaki, Short, Jeans, Jogger, QuanTay, DayLung, Vi, PhuKien, AoThun, Polo, Veston, Jacket, AoLen, Blazor, SoMi, Quan, Ao });

            foreach (JObject item in owen_obj)
            {
                Product product = new Product();
                product.Id = ObjectId.GenerateNewId().ToString();
                product.Name = item.GetValue("name").ToString();
                product.Price = double.Parse(item.GetValue("price").ToString().Replace(".", ","), CultureInfo.InvariantCulture);
                product.Images.Add(item.SelectToken("images[0].path").ToString());
                if (item.GetValue("name").ToString().Contains(ClothesCategory.SoMi, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = SoMi;
                    product.Category = Ao;

                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.Blazor, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = Blazor;
                    product.Category = Ao;

                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.AoLen, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = AoLen;
                    product.Category = Ao;

                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.Jacket, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = Jacket;
                    product.Category = Quan;

                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.Veston, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = Veston;
                    product.Category = Ao;

                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.Tshirt, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = AoThun;
                    product.Category = Ao;

                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.Polo, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = Polo;
                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.AoThun, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = AoThun;
                    product.Category = Ao;

                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.QuanAu, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = QuanAu;
                    product.Category = Quan;

                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.QuanKaki, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = Kaki;
                    product.Category = Quan;

                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.QuanShort, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = Short;
                    product.Category = Quan;

                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.QuanJeans, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = Jeans;
                    product.Category = Quan;

                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.QuanJogger, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = Jogger;
                    product.Category = Quan;

                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.QuanTay, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = QuanTay;
                    product.Category = Quan;

                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.DayLung, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = DayLung;
                    product.Category = PhuKien;

                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.Vi, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = Vi;
                    product.Category = PhuKien;

                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.Tat, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = Tat;
                    product.Category = PhuKien;
                }
                else
                {
                    product.SubCategory = PhuKien;
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

            JArray hem_obj = new JArray();
            using (StreamReader file = File.OpenText(@"C:\Users\QuyenTho\projects\clothes\hemstore_dataset.json"))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                hem_obj = (JArray)JToken.ReadFrom(reader);
            }
            foreach (JObject item in hem_obj)
            {
                Product product = new Product();
                product.Id = ObjectId.GenerateNewId().ToString();
                product.Name = item.GetValue("name").ToString();
                product.Price = double.Parse(item.GetValue("price").ToString(), CultureInfo.InvariantCulture);
                product.Images.Add(item.SelectToken("images[0].path").ToString());
                if (item.GetValue("name").ToString().Contains(ClothesCategory.SoMi, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = SoMi;
                    product.Category = Ao;

                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.AoThun, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = AoThun;
                    product.Category = Ao;

                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.Polo, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = Polo;
                }

                else if (item.GetValue("name").ToString().Contains(ClothesCategory.QuanAu, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = QuanAu;
                    product.Category = Quan;

                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.QuanKaki, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = Kaki;
                    product.Category = Quan;

                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.QuanShort, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = Short;
                    product.Category = Quan;

                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.QuanJeans, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = Jeans;
                    product.Category = Quan;

                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.QuanJogger, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = Jogger;
                    product.Category = Quan;

                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.QuanTay, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = QuanTay;
                    product.Category = Quan;

                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.DayLung, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = DayLung;
                    product.Category = PhuKien;

                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.Tat, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = Tat;
                    product.Category = PhuKien;

                }
                else if (item.GetValue("name").ToString().Contains(ClothesCategory.Vi, StringComparison.OrdinalIgnoreCase))
                {
                    product.SubCategory = Vi;
                    product.Category = PhuKien;
                }
                else
                {
                    product.SubCategory = PhuKien;
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

        public static class ClothesCategory
        {

            public const string Ao = "Áo";
            public const string Quan = "Quần";
            public const string SoMi = "sơ mi";
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
            public const string QuanJeans = "Quần Jean";
            public const string QuanJogger = "Quần Jogger";
            public const string QuanTay = "Quần Tây";
            public const string QuanLot = "Boxer";
            public const string DayLung = "Dây Lưng";
            public const string Vi = "Ví";
            public const string Tat = "Tất";
            public const string PhuKien = "Phụ Kiện";
        }
    }
}