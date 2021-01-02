using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoShop.BusinessDomain;
using MongoShop.BusinessDomain.Orders;
using MongoShop.BusinessDomain.Products;
using MongoShop.Models;

namespace MongoShop.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {

        private readonly IMongoCollection<Order> _collection;
        private readonly IDatabaseSetting _databaseSetting;
        private const string CollectionName = "order";
        private readonly IOrderServices _orderServices;

        public HomeController(IDatabaseSetting databaseSetting, IOrderServices orderServices)
        {
            _databaseSetting = databaseSetting;
            this._orderServices = orderServices;
            var client = new MongoClient(_databaseSetting.ConnectionString);
            var database = client.GetDatabase(_databaseSetting.DatabaseName);

            _collection = database.GetCollection<Order>(CollectionName);
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> IndexAsync()
        {
            var orders = new List<Order>()
            {
                new Order()
                {
                 Invoice = new Invoice() { Status = InvoiceStatus.Cancel},

                },
                new Order()
                {
                 Invoice = new Invoice() { Status = InvoiceStatus.Cancel},

                },new Order()
                {
                 Invoice = new Invoice() { Status = InvoiceStatus.Cancel},

                }
            };

            _collection.InsertMany(orders);

            var ordersPending =await _orderServices.GetOrdersWithUnpaidInvoiceAsync();
            
            return View();
        }

        public IActionResult Privacy()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
