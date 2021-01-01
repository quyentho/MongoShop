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

            var indexProductViewModel = _mapper.Map<List<IndexProductViewModel>>(products);

            return View(indexProductViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<Category> categories = await _categoryServices.GetAllAsync();

            ViewData["Categories"] = _mapper.Map<List<CategoryViewModel>>(categories);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                return await Create();
            }

            string categoryId = productViewModel.Category.Id;
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
            
            var categories = await _categoryServices.GetAllAsync();
            
            var editProductViewModel = _mapper.Map<EditProductViewModel>(product);

            editProductViewModel.CategoryList = _mapper.Map<List<SelectListItem>>(categories);

            return View(editProductViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, EditProductViewModel editProductViewModel)
        {
            if (!ModelState.IsValid)
            {
                return await Edit(id);
            }

            var editedProduct = _mapper.Map<Product>(editProductViewModel);

            if (editProductViewModel.ImagesUpload != null)
            {
                List<string> imagePaths = await _fileUploadService.Upload(editProductViewModel.ImagesUpload);
                editedProduct.Images.AddRange(imagePaths);
            }

            await _productServices.EditAsync(id, editedProduct);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var product = await _productServices.GetByIdAsync(id);

            var detailProductViewmodel = _mapper.Map<DetailProductViewModel>(product);

            return View(detailProductViewmodel);
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
    }
}
