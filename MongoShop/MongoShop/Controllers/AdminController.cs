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

        public AdminController(IProductServices productServices, IMapper mapper)
        {
            _productServices = productServices;
            this._mapper = mapper;
            
        }

        [HttpGet]
        public async Task<IActionResult> IndexProduct()
        {
            var products = await _productServices.GetAllAsync();

            var productsViewModels = _mapper.Map<List<ProductViewModel>>(products);
            
            return View(productsViewModels);
        }
        
        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductViewModel productViewModel)
        {
            _mapper.Map<Product>(productViewModel);
            
            return View();
        }

        public async Task<IActionResult> EditProduct(string id)
        {
            var product = await _productServices.GetByIdAsync(id);

            var productViewModel = _mapper.Map<ProductViewModel>(product);
            return View(productViewModel);
        }
    }
}
