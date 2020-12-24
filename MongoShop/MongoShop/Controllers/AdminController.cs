using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoShop.BusinessDomain.User;

namespace MongoShop.Controllers
{
    [Authorize(Roles = UserRole.Admin)]
    public class AdminController : Controller
    {
        public IActionResult Product()
        {
            return View();
        }
    }
}
