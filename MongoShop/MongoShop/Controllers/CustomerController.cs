using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoShop.BusinessDomain.Categories;
using MongoShop.BusinessDomain.Products;
using MongoShop.Models.Customer;
using MongoShop.Infrastructure.Services.FileUpload;
using MongoShop.Utils;

namespace MongoShop.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly IHomePageProductServices _homePageProductServices;
        private readonly ICategoryServices _categoryServices;
        private readonly IMapper _mapper;
        private readonly IFileUploadService _fileUploadService;

        public CustomerController(IProductServices productServices,
            IMapper mapper,
            ICategoryServices categoryServices,
            IFileUploadService fileUploadService, 
            IHomePageProductServices homePageProductServices)
        {
            _productServices = productServices;
            _mapper = mapper;
            _categoryServices = categoryServices;
            _fileUploadService = fileUploadService;
            _homePageProductServices = homePageProductServices;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var shirtCate =await _categoryServices.GetByNameAsync("Áo");
            var trouserCate =await _categoryServices.GetByNameAsync("Quần");
            var accessoriesCate =await _categoryServices.GetByNameAsync("Phụ Kiện");
            
            var shirts = await _homePageProductServices.GetByMainCategoryAsync(shirtCate);

            var shirtsViewModel = _mapper.Map<List<IndexViewModel>>(shirts);

            var trousers = await _homePageProductServices.GetByMainCategoryAsync(trouserCate);

            var trousersViewModel = _mapper.Map<List<IndexViewModel>>(trousers);

            var accessories = await _homePageProductServices.GetByMainCategoryAsync(accessoriesCate);

            var accessoriesViewModel = _mapper.Map<List<IndexViewModel>>(accessories);

            var model = new CustomerMultipleList()
            {
                ShirtCollection = shirtsViewModel,
                TrouserCollection = trousersViewModel,
                AccessoriesCollection = accessoriesViewModel
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Category(int currentPageNumber = 1)
        {
            var products = await _productServices.GetAllAsync();

            var indexProductViewModel = _mapper.Map<List<IndexViewModel>>(products);

            return View(PaginatedList<IndexViewModel>.CreateAsync(indexProductViewModel.AsQueryable(), currentPageNumber));
        }



        [HttpGet]
        public async Task<IActionResult> ProductDetail(string id)
        {
            var product = await _productServices.GetByIdAsync(id);

            var customerProductDetailViewModel = _mapper.Map<CustomerProductDetailViewModel>(product);

            return View(customerProductDetailViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Category(string[] tags, float? price_min, float? price_max)
        {
            var products = await _productServices.GetAllAsync();
            var indexProductViewModel = _mapper.Map<List<IndexViewModel>>(products);
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
    }
}
