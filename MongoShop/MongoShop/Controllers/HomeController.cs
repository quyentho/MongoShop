using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoShop.BusinessDomain.Products;
using MongoShop.BusinessDomain.Users;
using MongoShop.Models;

namespace MongoShop.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;


        public HomeController(IProductServices productServices, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this._productServices = productServices;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            //await _roleManager.CreateAsync(new ApplicationRole(UserRole.User));
            //Product product = new Product
            //{
            //    Name = "Product4",
            //    Price = 450,
            //    StockQuantity = 10,
            //    Size = "XL",
            //    Status = true,
            //    CreatedAt = DateTime.Now
            //};
            //await _productServices.AddAsync(product);
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
