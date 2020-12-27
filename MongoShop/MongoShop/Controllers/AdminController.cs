using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoShop.BusinessDomain.Products;
using MongoShop.Models.Admin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoShop.Controllers
{
    //[Authorize(Roles = UserRole.Admin)]
    public class AdminController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly IMapper _mapper;

        //private readonly IOrderServices _orderServices;
        public AdminController(IProductServices productServices, IMapper mapper)
        {
            _productServices = productServices;
            this._mapper = mapper;
            //_orderServices = orderServices;
        }

        [HttpGet]
        public async Task<IActionResult> Product()
        {
            var products = await _productServices.GetAllAsync();

            var productsViewModels = _mapper.Map<List<ProductViewModel>>(products);
            
            return View(productsViewModels);
        }
    }
}
