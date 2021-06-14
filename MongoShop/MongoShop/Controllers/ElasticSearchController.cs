using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoShop.BusinessDomain.Products;
using MongoShop.Models.Customer;
using MongoShop.Utils;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoShop.Controllers
{
    public class ElasticSearchController : Controller
    {
        private readonly IElasticClient _client;
        private readonly IMapper _mapper;

        public ElasticSearchController(IElasticClient client, IMapper mapper) { 
            
            _client = client;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult Search(string queryString, int pageNumber = 1)
        {
            ViewData["queryString"] = queryString;
            var result = _client.Search<Product>(s => s
                            .Size(25)
                            .Query(q => q
                                .MultiMatch(m => m
                                        .Fields(f => f
                                            .Field(p => p.Name.Suffix("keyword"), 2)
                                            .Field(p => p.Name, 1.3)
                                            .Field(p => p.Name.Suffix("normalize"), 1.2)
                                        )
                                    .Query(queryString)
                                )
                            )
                        );

            List<Product> products = new List<Product>();
            foreach (var item in result.Hits)
            {
                products.Add(item.Source);
            }
            

            var viewModels = _mapper.Map<List<IndexViewModel>>(products);

            return View("SearchedProducts", PaginatedList<IndexViewModel>.CreateAsync(viewModels.AsQueryable(), pageNumber));
        }
    }
}
