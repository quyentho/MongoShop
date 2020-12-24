using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoShop.BusinessDomain.Product;
using MongoShop.BusinessDomain.User;
using MongoShop.Models;

namespace MongoShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(IProductServices productServices, UserManager<ApplicationUser> userManager)
        {
            this._productServices = productServices;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = new ApplicationUser("thokimquangquyen@gmail.com", "thokimquangquyen@gmail.com");

            var result = await _userManager.CreateAsync(user);
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
