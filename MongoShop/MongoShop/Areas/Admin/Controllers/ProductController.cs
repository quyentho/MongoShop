using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IHomePageProductServices _homePageProductServices;
        private readonly IMapper _mapper;
        private readonly IFileUploadService _fileUploadService;

        public ProductController(IProductServices productServices,
            IMapper mapper,
            ICategoryServices categoryServices,
            IHomePageProductServices homePageProductServices,
            IFileUploadService fileUploadService)
        {
            _productServices = productServices;
            _mapper = mapper;
            _categoryServices = categoryServices;
            this._homePageProductServices = homePageProductServices;
            _fileUploadService = fileUploadService;
        }
        [HttpGet]
        public async Task<IActionResult> SelectMainPageProducts()
        {
            var mainCategories = await _categoryServices.GetAllMainCategoryAsync();
            mainCategories = (List<Category>)mainCategories.OrderBy(c => c.Name).ToList();

            AdminMainPageProductsViewModel adminMainPageProductsViewModel = new AdminMainPageProductsViewModel();
            foreach (var category in mainCategories)
            {
                var products = await _productServices.GetByMainCategoryAsync(category);
                var homePageProducts = await _homePageProductServices.GetByMainCategoryAsync(category);

                List<MainPageProductList> productViewModels = GetViewModel(products, homePageProducts);

                adminMainPageProductsViewModel.ListProduct.Add(productViewModels);
            }
            return View(adminMainPageProductsViewModel);

        }

        private List<MainPageProductList> GetViewModel(List<Product> products, List<Product> homePageProducts)
        {
            List<MainPageProductList> productViewModels = _mapper.Map<List<MainPageProductList>>(products);
            for (int i = 0; i < homePageProducts.Count; i++)
            {
                var selectedProduct = productViewModels.Find(c => c.ProductId == homePageProducts[i].Id);
                selectedProduct.IsSelected = true;
            }

            return productViewModels;
        }

        [HttpPost]
        public async Task<IActionResult> SelectMainPageProducts(string categoryId, string[] productIds)
        {
            if (string.IsNullOrEmpty(categoryId))
            {
                var mainCategories = await _categoryServices.GetAllMainCategoryAsync();
                categoryId = mainCategories.FirstOrDefault(c => c.Name.Contains("Áo"))?.Id;
            }

            if (productIds is null)
            {
                return BadRequest();
            }

            Category category = await _categoryServices.GetByIdAsync(categoryId);
            List<Product> products = new List<Product>();
            for (int i = 0; i < productIds.Length; i++)
            {
                products.Add(await _productServices.GetByIdAsync(productIds[i]));
            }

            await _homePageProductServices.AddAsync(products);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Index(int currentPageNumber = 1)
        {
            var products = await _productServices.GetAllAsync();

            var indexProductViewModels = _mapper.Map<List<IndexProductViewModel>>(products);

            return View(PaginatedList<IndexProductViewModel>.CreateAsync(indexProductViewModels.AsQueryable(), currentPageNumber));
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
    }
}
