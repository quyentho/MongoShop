using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoShop.BusinessDomain.Categories;
using MongoShop.BusinessDomain.Products;
using MongoShop.Infrastructure.Services.FileUpload;
using MongoShop.SharedModels.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MongoShop.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productServices;
        private readonly ICategoryServices _categoryServices;
        private readonly IMapper _mapper;
        private readonly IFileUploadService _fileUploadService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductServices productServices,
            IMapper mapper,
            ICategoryServices categoryServices,
            IFileUploadService fileUploadService,
            ILogger<ProductController> logger)
        {
            _productServices = productServices;
            _mapper = mapper;
            _categoryServices = categoryServices;
            _fileUploadService = fileUploadService;
            this._logger = logger;
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>List products found.</returns>
        [HttpGet]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        [EnableCors("Policy1")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _productServices.GetAllAsync();
                if (products is null || products.Count == 0)
                {
                    _logger.LogInformation("There is no product available");

                    return NotFound("There is no product available");
                }

                var productViewModels = _mapper.Map<List<ProductViewModel>>(products);

                return Ok(productViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when getting all products.");

                return StatusCode(StatusCodes.Status500InternalServerError,
                        "Error retrieving data from the database");
            }

        }
        /// <summary>
        /// Create new product.
        /// </summary>
        /// <param name="createProductRequest">Product info.</param>
        /// <returns>201 created.</returns>
        [HttpPost]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                    nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> Create([FromForm] CreateProductRequest createProductRequest)
        {
            try
            {

                // upload image and get back the paths
                List<string> imagePaths = await _fileUploadService.Upload(createProductRequest.ImagesUpload);

                var product = _mapper.Map<Product>(createProductRequest);

                product.Images = imagePaths;

                await _productServices.AddAsync(product);

                return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when create product.");


                return StatusCode(StatusCodes.Status500InternalServerError,
                  "Server error occurred when create product");
            }
        }

        /// <summary>
        /// Updates a product.
        /// </summary>
        /// <param name="id">product id.</param>
        /// <param name="editProductRequest">Updated product.</param>
        /// <returns>204 No content.</returns>
        [HttpPut]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                    nameof(DefaultApiConventions.Put))]
        public async Task<IActionResult> Edit(string id, [FromForm] EditProductRequest editProductRequest)
        {
            try
            {
                if (id != editProductRequest.Id)
                {
                    _logger.LogInformation("product ID: {cID} mismatch with param id: {id}", editProductRequest.Id, id);
                    return BadRequest("Category ID mismatch");
                }

                var editedProduct = _mapper.Map<Product>(editProductRequest);

                if (editProductRequest.ImagesUpload != null)
                {
                    _logger.LogInformation("Upload images:");

                    List<string> imagePaths = await _fileUploadService.Upload(editProductRequest.ImagesUpload);
                    editedProduct.Images.AddRange(imagePaths);
                }

                await _productServices.EditAsync(id, editedProduct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when update product.");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
            
        }

        /// <summary>
        /// Gets product by id.
        /// </summary>
        /// <param name="id">Product id.</param>
        /// <returns>Product found.</returns>
        [HttpGet]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<ProductViewModel>> GetById([Required, StringLength(24, MinimumLength = 24, ErrorMessage = "Id must be 24 digits string")] string id)
        {
            try
            {
                var product = await _productServices.GetByIdAsync(id);
                if (product is null)
                {
                    _logger.LogInformation("There is no product with id: {id}", id);

                    return NotFound($"There is no product with id: {id}");
                }

                var productViewmodel = _mapper.Map<ProductViewModel>(product);

                return Ok(productViewmodel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when get product id: {id}.",id);

                return StatusCode(StatusCodes.Status500InternalServerError,
                  "Error retrieving data from the database");
            }
        }

        /// <summary>
        /// Soft delete a product.
        /// </summary>
        /// <param name="id">Product id.</param>
        /// <returns>200 Ok.</returns>
        [HttpDelete]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Delete))]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var product = await _productServices.GetByIdAsync(id);

                if (product is null)
                {
                    _logger.LogInformation("There is no product with id: {id}", id);

                    return BadRequest($"Product with id: {id} not found.");
                }

                await _productServices.DeleteAsync(id, product);
                return Ok($"Product with id: {id} deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when delete product.");

                return StatusCode(StatusCodes.Status500InternalServerError,
                 "Error retrieving data from the database");
            }
        }
    }
}
