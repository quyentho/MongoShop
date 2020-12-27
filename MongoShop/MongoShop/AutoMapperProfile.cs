using AutoMapper;
using MongoShop.BusinessDomain.Categories;
using MongoShop.BusinessDomain.Products;
using MongoShop.BusinessDomain.Users;
using MongoShop.Models.Account;
using MongoShop.Models.Admin;

namespace MongoShop
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, ProductViewModel>();
            CreateMap<Category, CategoryViewModel>();
        }
    }
}