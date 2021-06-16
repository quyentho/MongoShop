using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoShop.BusinessDomain.Products;
using MongoShop.Infrastructure.Services.FileUpload;
using MongoShop.Models.Customer;
using MongoShop.Utils;
using Newtonsoft.Json;
using RestSharp;
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
        private readonly IProductServices _productServices;

        public ImagesSearchController(IFileUploadService fileUploadService, IMapper mapper, IProductServices productServices)
        {
            _fileUploadService = fileUploadService;
            _mapper = mapper;
            _productServices = productServices;
        }


        [HttpGet]
        public IActionResult DisplaySearchResult(List<Product> products, int pageNumber = 1)
        {
            ViewData["products"] = products;
            var viewModels = _mapper.Map<List<IndexViewModel>>(products);
            return View("SearchedProducts", PaginatedList<IndexViewModel>.CreateAsync(viewModels.AsQueryable(), pageNumber));
        }

        [HttpPost]
        public async Task<IActionResult> SearchForSimilar([FromForm] IFormFile imageUpload)
        {
            // upload image
            List<string> imagePaths = await _fileUploadService.Upload(new List<IFormFile>() { imageUpload });

            var imagePath = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{imagePaths[0]}";
            // 64 base encode
            string encodedStr = Convert.ToBase64String(Encoding.UTF8.GetBytes(imagePath));

            // use RestSharp to make http request
            //calling the api
            var client = new RestClient("http://127.0.0.1:5000/");

            var request = new RestRequest("/SearchImage/{img_path}/")
                .AddUrlSegment("img_path", encodedStr);

            var response = client.Get(request);

            //get the json object
            var json = JsonConvert.DeserializeObject<List<string>>(response.Content);

            List<string> pathlist = new List<string>();

            //add to the list
            foreach (var path in json)
            {
                pathlist.Add(path);
            }

            List<Product> products = new List<Product>();

            // find product by image path
            foreach (var path in pathlist)
            {
                 products.Add(await _productServices.GetByImageAsync(path));
            }


            // return view
            
            return RedirectToAction(nameof(DisplaySearchResult), products);
        }
    }
}
