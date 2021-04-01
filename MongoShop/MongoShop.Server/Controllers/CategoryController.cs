using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoShop.BusinessDomain.Categories;
using MongoShop.Server.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoShop.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryServices categoryServices, IMapper mapper)
        {
            _categoryServices = categoryServices;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all Categories.
        /// </summary>
        /// <returns>List categories if any</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            
            var categories = await _categoryServices.GetAllAsync();
            if (categories is null)
            {
                return NotFound("There is no category available");
            }

            var indexCategoryViewModels = _mapper.Map<List<IndexCategoryViewModel>>(categories);

            return Ok(indexCategoryViewModels);
        }
    }
}
