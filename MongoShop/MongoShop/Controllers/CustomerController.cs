using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoShop.BusinessDomain.Categories;
using MongoShop.BusinessDomain.Products;
using MongoShop.Services.FileUpload;
using MongoShop.Areas.Admin.ViewModels.Product;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace MongoShop.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly ICategoryServices _categoryServices;
        private readonly IMapper _mapper;
        private readonly IFileUploadService _fileUploadService;

        public CustomerController(IProductServices productServices,
            IMapper mapper,
            ICategoryServices categoryServices,
            IFileUploadService fileUploadService)
        {
            _productServices = productServices;
            _mapper = mapper;
            _categoryServices = categoryServices;
            _fileUploadService = fileUploadService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productServices.GetAllAsync();

            var indexProductViewModel = _mapper.Map<List<IndexProductViewModel>>(products);

            return View(indexProductViewModel.Take(3));
        }

        [HttpGet]
        public async Task<IActionResult> Category()
        {
            var products = await _productServices.GetAllAsync();

            var indexProductViewModel = _mapper.Map<List<IndexProductViewModel>>(products);

            return View(indexProductViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ProductDetail(string id)
        {
            var product = await _productServices.GetByIdAsync(id);


            var detailProductViewModel = _mapper.Map<DetailProductViewModel>(product);

            return View(detailProductViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Category(string[] tags, float? price_min, float? price_max)
        {
            var products = await _productServices.GetAllAsync();
            var indexProductViewModel = _mapper.Map<List<IndexProductViewModel>>(products);
            var result = indexProductViewModel.Where(m=>m.Price > price_min && m.Price<price_max);
            if(tags.Length == 1)
            {
                return View(result.Where(m => m.Category == tags[0]));
            }
            else if(tags.Length == 2)
            {
                return View(result.Where(m => m.Category == tags[0] || m.Category == tags[1]));
            }

            return View(result);
        }

        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult Checkout()
        {
            return View();
        }

    }
}
