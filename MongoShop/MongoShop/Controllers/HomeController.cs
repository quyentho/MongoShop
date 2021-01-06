using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoShop.BusinessDomain;
using MongoShop.BusinessDomain.Orders;
using MongoShop.BusinessDomain.Products;
using MongoShop.BusinessDomain.Users;
using MongoShop.Models;

namespace MongoShop.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IUserServices userServices;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public HomeController(IUserServices userServices, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this.userServices = userServices;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ApplicationUser user = new ApplicationUser()
            {
                Email = "admin@admin.com",
                UserName = "admin@admin.com",
                Name = "admin"
            };
            
           var result = userManager.CreateAsync(user,"123456").GetAwaiter().GetResult() ;

           var result2 = userManager.AddToRoleAsync(user, UserRole.Admin).GetAwaiter().GetResult();

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
