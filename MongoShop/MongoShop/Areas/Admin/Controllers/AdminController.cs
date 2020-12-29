using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoShop.BusinessDomain.Products;

namespace MongoShop.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly IMapper _mapper;

        public AdminController(IProductServices productServices, IMapper mapper)
        {
            _productServices = productServices;
            _mapper = mapper;

        }


    }
}
