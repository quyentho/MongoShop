using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoShop.BusinessDomain.Orders;
using MongoShop.BusinessDomain.Products;
using MongoShop.BusinessDomain.Users;
using System.Threading.Tasks;

namespace MongoShop.Controllers
{
    [Authorize(Roles = UserRole.Admin)]
    public class AdminController : Controller
    {
        private readonly IProductServices _productServices;
        //private readonly IOrderServices _orderServices;
        public AdminController(IProductServices productServices)
        {
            _productServices = productServices;
            //_orderServices = orderServices;
        }
        public async Task<IActionResult> Product()
        {
            await _productServices.GetAllAsync();
            return View();
        }
    }
}
