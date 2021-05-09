using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoShop.Areas.Admin.ViewModels.Product;
using MongoShop.BusinessDomain.Categories;
using MongoShop.BusinessDomain.Products;
using MongoShop.Infrastructure.Services.FileUpload;
using MongoShop.Utils;

namespace MongoShop.Areas.Admin.Controllers
{

    [Area("Admin")]
    //[Authorize]

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

        [HttpGet()]
        public async Task<IActionResult> Index(int currentPageNumber = 1)
        {
            var products = await _productServices.GetAllAsync();

            var indexProductViewModels = _mapper.Map<List<IndexProductViewModel>>(products);

           return View( PaginatedList<IndexProductViewModel>.CreateAsync(indexProductViewModels.AsQueryable(), currentPageNumber));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var createProductViewModel = new CreateProductViewModel();

            List<Category> categories = await _categoryServices.GetAllMainCategoryAsync();

            createProductViewModel.CategoryList = _mapper.Map<List<SelectListItem>>(categories);

            return View(createProductViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                return await Create();
            }

            // upload image and get back the paths
            List<string> imagePaths = await _fileUploadService.Upload(productViewModel.ImagesUpload);

            var product = _mapper.Map<Product>(productViewModel);

            product.Images = imagePaths;

            await _productServices.AddAsync(product);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var product = await _productServices.GetByIdAsync(id);
            
            
            var editProductViewModel = _mapper.Map<EditProductViewModel>(product);

            var categories = await _categoryServices.GetAllMainCategoryAsync();

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

        [HttpGet]
        public async Task<IActionResult> SelectMainPageProducts()
        {
            return View();
        }
    }
}
