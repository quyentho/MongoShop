using AutoMapper;
using MongoShop.Areas.Admin.ViewModels.Category;
using MongoShop.Areas.Admin.ViewModels.Product;
using MongoShop.BusinessDomain.Categories;
using MongoShop.BusinessDomain.Products;

namespace MongoShop
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, CreateProductViewModel>();

            CreateMap<CreateProductViewModel, Product>();

            CreateMap<Product, DisplayProductViewModel>();

            CreateMap<CreateProductViewModel, DisplayProductViewModel>();

            CreateMap<Category, CategoryViewModel>();
        }
    }
}