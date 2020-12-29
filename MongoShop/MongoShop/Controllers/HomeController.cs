using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoShop.BusinessDomain.Categories;
using MongoShop.BusinessDomain.Products;
using MongoShop.Models;

namespace MongoShop.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly ICategoryServices _categoryServices;


        public HomeController(IProductServices productServices, ICategoryServices categoryServices)
        {
            this._productServices = productServices;
            this._categoryServices = categoryServices;
        }

        public async Task<IActionResult> Index()
        {
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
