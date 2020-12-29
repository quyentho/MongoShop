using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoShop.Areas.Admin.ViewModels.Category;
using MongoShop.Areas.Admin.ViewModels.Product;
using MongoShop.BusinessDomain.Categories;
using MongoShop.BusinessDomain.Products;
using MongoShop.Services.FileUpload;

namespace MongoShop.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly ICategoryServices _categoryServices;
        private readonly IMapper _mapper;
        private readonly IFileUploadService _fileUploadService;

        public ProductController(IProductServices productServices,
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

            var displayProductViewModel = _mapper.Map<List<DisplayProductViewModel>>(products);

            return View(displayProductViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<Category> categories = await _categoryServices.GetAllAsync();

            ViewData["Categories"] = _mapper.Map<List<CategoryViewModel>>(categories);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductViewModel productViewModel, string categoryId)
        {

            var category = await _categoryServices.GetByIdAsync(categoryId);

            // upload image and get back the paths
            List<string> imagePaths = await _fileUploadService.Upload(productViewModel.Images);

            var product = _mapper.Map<Product>(productViewModel);
            
            product.Images = imagePaths;

            await _productServices.AddAsync(product, categoryId);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditProduct(string id)
        {
            var product = await _productServices.GetByIdAsync(id);

            var displayProductViewModel = _mapper.Map<DisplayProductViewModel>(product);
            return View(displayProductViewModel);
        }
    }
}
