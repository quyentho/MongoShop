using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoShop.BusinessDomain.Product;
using MongoShop.Models;

namespace MongoShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductServices _productServices;

        public HomeController(IProductServices productServices)
        {
            this._productServices = productServices;
        }

        public async Task<IActionResult> Index()
        {
            await _productServices.AddAsync(new Product() { Name = "Test" });
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
