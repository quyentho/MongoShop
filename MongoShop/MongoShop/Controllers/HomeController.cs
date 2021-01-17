using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentEmail.Core;
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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public HomeController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromServices] IFluentEmail email)
        {
            ApplicationRole adminRole = await roleManager.FindByNameAsync(UserRole.Admin);
            if (adminRole == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(UserRole.Admin));
            }

            var user = new ApplicationUser { Id = Guid.NewGuid(),UserName = "admin@admin.com", Email = "admin@admin.com", Status = true };
            await userManager.CreateAsync(user, "123456");
            await userManager.AddToRoleAsync(user, UserRole.Admin);

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
