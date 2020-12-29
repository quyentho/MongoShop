﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoShop.Areas.Admin.ViewModels.Product;
using MongoShop.BusinessDomain.Products;

namespace MongoShop.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly IMapper _mapper;

        public ProductController(IProductServices productServices, IMapper mapper)
        {
            _productServices = productServices;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var products = await _productServices.GetAllAsync();

            var displayProductViewModel = _mapper.Map<List<DisplayProductViewModel>>(products);

            return View(displayProductViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductViewModel productViewModel)
        {
            var product = _mapper.Map<Product>(productViewModel);

            await _productServices.AddAsync(product);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditProduct(string id)
        {
            var product = await _productServices.GetByIdAsync(id);

            var displayProductViewModel = _mapper.Map<DisplayProductViewModel>(product);
            return View(displayProductViewModel);
        }
    }
}
