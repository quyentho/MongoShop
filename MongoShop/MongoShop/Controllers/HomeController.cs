using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoShop.BusinessDomain.Wishlists;
using MongoShop.Models;
using MongoShop.Services.FileUpload;

namespace MongoShop.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IWishlistServices _wishlistServices;

        public HomeController(IWishlistServices wishlistServices)
        {
            this._wishlistServices = wishlistServices;
        }

        [HttpPost]
        public async Task<IActionResult> Index(List<IFormFile> images)
        {
          

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var wishlist =await _wishlistServices.GetWishlistItemsByUserIdAsync("783c3082-3970-4428-ad53-d17382a2a1c8");
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
