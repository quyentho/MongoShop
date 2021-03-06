﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoShop.BusinessDomain.Categories;
using MongoShop.Server.ViewModels.Category;
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
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryServices _categoryServices;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryServices categoryServices, IMapper mapper, ILogger<CategoryController> logger)
        {
            _categoryServices = categoryServices;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all Categories.
        /// </summary>
        /// <returns>List categories if any</returns>
        [HttpGet]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<List<CategoryViewModel>>> GetAllMainCategoryAsync()
        {

            try
            {
                var categories = await _categoryServices.GetAllMainCategoryAsync();
                if (categories is null || categories.Count == 0)
                {
                    _logger.LogInformation("There is no category available");

                    return NotFound("There is no category available");
                }

                var categoryViewModels = _mapper.Map<List<CategoryViewModel>>(categories);

                return Ok(categoryViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when getting all categories.");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        /// <summary>
        /// Get all Categories.
        /// </summary>
        /// <returns>List categories if any</returns>
        [HttpGet]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<List<CategoryViewModel>>> GetAllSubCategoryAsync()
        {

            try
            {
                var categories = await _categoryServices.GetAllSubCategoryAsync();
                if (categories is null || categories.Count == 0)
                {
                    _logger.LogInformation("There is no category available");

                    return NotFound("There is no category available");
                }

                var categoryViewModels = _mapper.Map<List<CategoryViewModel>>(categories);

                return Ok(categoryViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when getting all categories.");

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
        public async Task<ActionResult<CategoryViewModel>> GetById([Required, StringLength(24, MinimumLength = 24, ErrorMessage = "Id must be 24 digits string")] string id)
        {

            try
            {
                var category = await _categoryServices.GetByIdAsync(id);

                if (category is null)
                {
                    _logger.LogInformation("Category with Id = {id} not found", id);

                    return NotFound($"Category with Id = {id} not found");
                }

                var detailCategoryViewmodel = _mapper.Map<CategoryViewModel>(category);

                return Ok(detailCategoryViewmodel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when get category id: {id}.", id);

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
        public async Task<IActionResult> Create(CreateCategoryRequest newCategory)
        {
            try
            {
                var category = _mapper.Map<Category>(newCategory);

                category = await _categoryServices.AddAsync(category);

                var createdCategory = _mapper.Map<CategoryViewModel>(category);

                return CreatedAtAction(nameof(GetById), new { id = category.Id }, createdCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when create product.");


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
        public async Task<ActionResult> Delete([Required, StringLength(24, MinimumLength = 24, ErrorMessage = "Id must be 24 digits string")] string id)
        {
            try
            {
                var categoryToDelete = await _categoryServices.GetByIdAsync(id);

                if (categoryToDelete == null)
                {
                    _logger.LogInformation("Category with Id = {id} not found", id);

                    return NotFound($"Category with Id = {id} not found");
                }

                await _categoryServices.DeleteAsync(id);

                return Ok($"Category with Id = {id} deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when delete product.");

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
        public async Task<IActionResult> Update([Required, StringLength(24, MinimumLength = 24, ErrorMessage = "Id must be 24 digits string")] string id, EditCategoryRequest category)
        {
            try
            {
                if (id != category.Id)
                {
                    _logger.LogInformation("category ID: {cID} mismatch with param id: {id}", category.Id, id);
                    return BadRequest("Category ID mismatch");
                }

                var categoryFromDb = await _categoryServices.GetByIdAsync(id);

                if (categoryFromDb == null)
                {
                    _logger.LogInformation("Category with Id = {id} not found", id);
                    return NotFound($"Category with Id = {id} not found");
                }

                var editedCategory = _mapper.Map<Category>(category);

                await _categoryServices.EditAsync(id, editedCategory);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when update product.");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }
    }
}
