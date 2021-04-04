using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoShop.BusinessDomain.Categories;
using MongoShop.Server.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MongoShop.Server.Controllers
{
    [Route("api/[controller]/[action]")]
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
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetAll()
        {

            try
            {
                var categories = await _categoryServices.GetAllAsync();
                if (categories is null)
                {
                    return NotFound("There is no category available");
                }

                var indexCategoryViewModels = _mapper.Map<List<CategoryViewModel>>(categories);

                return Ok(indexCategoryViewModels);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        /// <summary>
        /// Gets category detail by id.
        /// </summary>
        /// <param name="id">Category id, must be 24 digits string.</param>
        /// <returns>A model of type <typeparamref name="DetailCategoryViewModel"/>></returns>
        [HttpGet("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<DetailCategoryViewModel>> GetById([Required, StringLength(24,MinimumLength = 24, ErrorMessage = "Id must be 24 digits string")] string id)
        {

            try
            {
                var category = await _categoryServices.GetByIdAsync(id);

                if (category is null)
                {
                    return NotFound();
                }

                var detailCategoryViewmodel = _mapper.Map<DetailCategoryViewModel>(category);

                return Ok(detailCategoryViewmodel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        /// <summary>
        /// Create new category.
        /// </summary>
        /// <param name="newCategory">New Category.</param>
        /// <returns>Status code 201 if created successfully.</returns>
        [HttpPost]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> Create(CreateCategoryViewModel newCategory)
        {
            try
            {
                var category = _mapper.Map<Category>(newCategory);

                category = await _categoryServices.AddAsync(category);

                return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Error when attempt to create category");
            }
        }

        /// <summary>
        /// Delete category by id.
        /// </summary>
        /// <param name="id">Category Id.</param>
        /// <returns>200 deleted successfully.</returns>
        [HttpDelete("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Delete))]
        public async Task<ActionResult> Delete([Required, StringLength(24,MinimumLength = 24, ErrorMessage = "Id must be 24 digits string")] string id)
        {
            try
            {
                var categoryToDelete = await _categoryServices.GetByIdAsync(id);

                if (categoryToDelete == null)
                {
                    return NotFound($"Category with Id = {id} not found");
                }

                await _categoryServices.DeleteAsync(id);

                return Ok($"Category with Id = {id} deleted");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }

        /// <summary>
        /// Update category.
        /// </summary>
        /// <param name="id">Category Id.</param>
        /// <param name="category">Updated category.</param>
        /// <returns>204 no content.</returns>
        [HttpPut("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Update))]
        public async Task<ActionResult<Category>> Update([Required, StringLength(24,MinimumLength = 24, ErrorMessage = "Id must be 24 digits string")] string id, EditCategoryViewModel category)
        {
            try
            {
                if (id != category.Id)
                    return BadRequest("Category ID mismatch");

                var categoryFromDb = await _categoryServices.GetByIdAsync(id);

                if (categoryFromDb == null)
                    return NotFound($"Category with Id = {id} not found");

                var editedCategory = _mapper.Map<Category>(category);

                await _categoryServices.EditAsync(id, editedCategory);

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }
    }
}
