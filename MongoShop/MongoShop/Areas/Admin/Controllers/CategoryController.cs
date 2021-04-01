using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoShop.Areas.Admin.ViewModels.Category;
using MongoShop.BusinessDomain.Categories;

namespace MongoShop.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryServices _categoryServices;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryServices categoryServices,IMapper mapper)
        {
            _categoryServices = categoryServices;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryServices.GetAllAsync();

            var indexCategoryViewModels = _mapper.Map<List<IndexCategoryViewModel>>(categories);

            return View(indexCategoryViewModels);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryViewModel categoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Create();
            }

            var category = _mapper.Map<Category>(categoryViewModel);

            await _categoryServices.AddAsync(category);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var category = await _categoryServices.GetByIdAsync(id);

            var editCategoryViewModel = _mapper.Map<EditCategoryViewModel>(category);

            return View(editCategoryViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, EditCategoryViewModel editCategoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                return await Edit(id);
            }

            var editedCategory = _mapper.Map<Category>(editCategoryViewModel);

            await _categoryServices.EditAsync(id, editedCategory);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var category = await _categoryServices.GetByIdAsync(id);

            var detailCategoryViewmodel = _mapper.Map<DetailCategoryViewModel>(category);

            return View(detailCategoryViewmodel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var category = await _categoryServices.GetByIdAsync(id);

            if (category is null)
            {
                ModelState.AddModelError(string.Empty, "Category is not existing");
                return await Index();
            }

            await _categoryServices.DeleteAsync(id, category);
            return RedirectToAction(nameof(Index));
        }
    }
}
