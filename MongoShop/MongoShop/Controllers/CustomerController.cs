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
using System;
using MongoShop.BusinessDomain.Carts;
using Microsoft.AspNetCore.Identity;
using MongoShop.BusinessDomain.Users;
using MongoShop.BusinessDomain.Orders;

namespace MongoShop.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly IHomePageProductServices _homePageProductServices;
        private readonly ICategoryServices _categoryServices;
        private readonly IMapper _mapper;
        private readonly IFileUploadService _fileUploadService;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICartServices _cartServices;

        public CustomerController(IProductServices productServices,
            IMapper mapper,
            ICategoryServices categoryServices,
            IFileUploadService fileUploadService,
            IHomePageProductServices homePageProductServices,
            ICartServices cartServices, 
            UserManager<ApplicationUser> userManager)
        {
            _productServices = productServices;
            _mapper = mapper;
            _categoryServices = categoryServices;
            _fileUploadService = fileUploadService;
            _homePageProductServices = homePageProductServices;
            _cartServices = cartServices;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var shirtCate = await _categoryServices.GetByNameAsync("Áo");
            var trouserCate = await _categoryServices.GetByNameAsync("Quần");
            var accessoriesCate = await _categoryServices.GetByNameAsync("Phụ Kiện");

            var shirts = await _homePageProductServices.GetByMainCategoryAsync(shirtCate);
            ViewData["shirtCateId"] = shirtCate.Id;

            var shirtsViewModel = _mapper.Map<List<IndexViewModel>>(shirts);

            var trousers = await _homePageProductServices.GetByMainCategoryAsync(trouserCate);
            ViewData["trousersCateId"] = trouserCate.Id;

            var trousersViewModel = _mapper.Map<List<IndexViewModel>>(trousers);

            var accessories = await _homePageProductServices.GetByMainCategoryAsync(accessoriesCate);
            ViewData["accessoriesCateId"] = accessoriesCate.Id;

            var accessoriesViewModel = _mapper.Map<List<IndexViewModel>>(accessories);

            var model = new CustomerMultipleList()
            {
                ShirtCollection = shirtsViewModel,
                TrouserCollection = trousersViewModel,
                AccessoriesCollection = accessoriesViewModel
            };


            // get cart items count
            await SetCartCount();

            return View(model);
        }

        private async Task SetCartCount()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            if (!string.IsNullOrEmpty(userId))
            {
                List<OrderedProduct> cartItems = await _cartServices.GetItemsByUserIdAsync(userId);
                ViewData["CartCount"] = 0;
                if (cartItems != null && cartItems.Count != 0)
                {
                    int count = 0;
                    foreach (var item in cartItems)
                    {
                        count += item.OrderedQuantity;
                    }

                    ViewData["CartCount"] = count;
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Category(int currentPageNumber = 1)
        {
            //var products = await _productServices.GetAllAsync();
            var products = await _productServices.GetByImageAsync("E:\\Tieu Luan\\MongoShop\\MongoShop\\wwwroot\\images\\full\\0a82e722c4549f93abe53ffbb59df536d989b0d2.jpg");

            var indexProductViewModel = _mapper.Map<List<IndexViewModel>>(products);

            return View("CategorizedProducts", PaginatedList<IndexViewModel>.CreateAsync(indexProductViewModel.AsQueryable(), currentPageNumber));
        }

        [HttpGet]
        public async Task<IActionResult> ProductDetail(string id)
        {
            var product = await _productServices.GetByIdAsync(id);


            var customerProductDetailViewModel = _mapper.Map<CustomerProductDetailViewModel>(product);
             
            await SetCartCount();

            return View(customerProductDetailViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> MainCategoryProducts(string categoryId, int pageNumber = 1)
        {
            ViewData["categoryId"] = categoryId;
            ViewData["isMainCate"] = true;
            
            await SetCartCount();
            
            var category = await _categoryServices.GetByIdAsync(categoryId);
            
            var products = await _productServices.GetByMainCategoryAsync(category);

            var viewModels = _mapper.Map<List<IndexViewModel>>(products);

            return View("CategorizedProducts", PaginatedList<IndexViewModel>.CreateAsync(viewModels.AsQueryable(), pageNumber));
        }

        [HttpGet]
        public async Task<IActionResult> SubCategoryProducts(string categoryId, int pageNumber = 1)
        {
            ViewData["categoryId"] = categoryId;
            ViewData["isMainCate"] = false;

             await SetCartCount();

            var category = await _categoryServices.GetByIdAsync(categoryId);

            var products = await _productServices.GetBySubCategoryAsync(category);

            var viewModels = _mapper.Map<List<IndexViewModel>>(products);

            return View("CategorizedProducts", PaginatedList<IndexViewModel>.CreateAsync(viewModels.AsQueryable(), pageNumber));
        }
    }
}
