using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoShop.BusinessDomain.Products;
using MongoShop.Infrastructure.Services.FileUpload;
using MongoShop.Models.Customer;
using MongoShop.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoShop.Controllers
{
    public class ImagesSearchController : Controller
    {
        private readonly IFileUploadService _fileUploadService;
        private readonly IMapper _mapper;

        public ImagesSearchController(IFileUploadService fileUploadService, IMapper mapper)
        {
            _fileUploadService = fileUploadService;
            _mapper = mapper;
        }


        [HttpGet]
        public IActionResult DisplaySearchResult(List<Product> products, int pageNumber = 1)
        {
            ViewData["products"] = products;
            var viewModels = _mapper.Map<List<IndexViewModel>>(products);
            return View("SearchedProducts", PaginatedList<IndexViewModel>.CreateAsync(viewModels.AsQueryable(), 1));
        }

        [HttpPost]
        public async Task<IActionResult> SearchForSimilar([FromForm] IFormFile imageUpload)
        {
            // upload image
            List<string> imagePaths = await _fileUploadService.Upload(new List<IFormFile>() { imageUpload });
            
            // 64 base encode
            string encodedStr = Convert.ToBase64String(Encoding.UTF8.GetBytes(imagePaths[0]));

            // use RestSharp to make http request

            // get back the return

            // find product by image path


            List<Product> products = new List<Product>();

            // return view

            
            return RedirectToAction(nameof(DisplaySearchResult), products);
        }
    }
}
