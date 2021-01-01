using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

            var indexProductViewModels = _mapper.Map<List<IndexProductViewModel>>(products);

            return View(indexProductViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<Category> categories = await _categoryServices.GetAllAsync();

            ViewData["Categories"] = _mapper.Map<List<CategoryViewModel>>(categories);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                return await Create();
            }

            string categoryId = productViewModel.SelectedCategoryId;
            var category = await _categoryServices.GetByIdAsync(categoryId);

            // upload image and get back the paths
            List<string> imagePaths = await _fileUploadService.Upload(productViewModel.Images);

            var product = _mapper.Map<Product>(productViewModel);

            product.Images = imagePaths;

            await _productServices.AddAsync(product, categoryId);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var product = await _productServices.GetByIdAsync(id);

            var productViewmodel = await PrepareProductData(product);

            return View(productViewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, ProductViewModel ProductViewModel)
        {
            if (!ModelState.IsValid)
            {
                return await Edit(id);
            }

            var editedProduct = _mapper.Map<Product>(ProductViewModel);

            await _productServices.EditAsync(id, editedProduct);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var product = await _productServices.GetByIdAsync(id);

            var productViewmodel = await PrepareProductData(product);

            return View(productViewmodel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var product = await _productServices.GetByIdAsync(id);

            if (product is null)
            {
                ModelState.AddModelError(string.Empty, "Product is not existing");
                return await Index();
            }

            await _productServices.DeleteAsync(id, product);
            return RedirectToAction(nameof(Index));
        }

        private async Task<ProductViewModel> PrepareProductData(Product product)
        {
            List<Category> categories = await _categoryServices.GetAllAsync();

            _mapper.Map<List<SelectListItem>>(categories);

            ViewData["imagePaths"] = product.Images;

            ViewData["productId"] = product.Id;

            return _mapper.Map<ProductViewModel>(product);
        }
    }
}
