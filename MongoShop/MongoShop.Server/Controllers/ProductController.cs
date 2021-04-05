using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoShop.Areas.Admin.ViewModels.Product;
using MongoShop.BusinessDomain.Categories;
using MongoShop.BusinessDomain.Products;
using MongoShop.Infrastructure.Services.FileUpload;
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

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>List products found.</returns>
        [HttpGet]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _productServices.GetAllAsync();
                if (products is null)
                {
                    return NotFound("There is no product available");
                }

                var productViewModels = _mapper.Map<List<ProductViewModel>>(products);

                return Ok(productViewModels);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                        "Error retrieving data from the database");
            }

        }
        /// <summary>
        /// Create new product.
        /// </summary>
        /// <param name="productViewModel">Product info.</param>
        /// <returns>201 created.</returns>
        [HttpPost]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                    nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> Create([FromForm]CreateProductViewModel productViewModel)
        {
            try
            {

                // upload image and get back the paths
                List<string> imagePaths = await _fileUploadService.Upload(productViewModel.ImagesUpload);

                var product = _mapper.Map<Product>(productViewModel);

                product.Images = imagePaths;

                await _productServices.AddAsync(product);

                return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  "Server error occurred when create product");
            }
        }

        /// <summary>
        /// Updates a product.
        /// </summary>
        /// <param name="id">product id.</param>
        /// <param name="editProductViewModel">Updated product.</param>
        /// <returns>204 No content.</returns>
        [HttpPut]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                    nameof(DefaultApiConventions.Put))]
        public async Task<IActionResult> Edit(string id,[FromForm] EditProductViewModel editProductViewModel)
        {
            if (id != editProductViewModel.Id)
            {
                return BadRequest();
            }

            var editedProduct = _mapper.Map<Product>(editProductViewModel);

            if (editProductViewModel.ImagesUpload != null)
            {
                List<string> imagePaths = await _fileUploadService.Upload(editProductViewModel.ImagesUpload);
                editedProduct.Images.AddRange(imagePaths);
            }

            await _productServices.EditAsync(id, editedProduct);

            return NoContent();
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
                    return NotFound($"There is no product with id: {id}");
                }

                var productViewmodel = _mapper.Map<ProductViewModel>(product);

                return Ok(productViewmodel);
            }
            catch (Exception)
            {
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
                    return BadRequest($"Product with id: {id} not found.");
                }

                await _productServices.DeleteAsync(id, product);
                return Ok("Product deleted successfully");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                 "Error retrieving data from the database");
            }

         
        }
    }
}
