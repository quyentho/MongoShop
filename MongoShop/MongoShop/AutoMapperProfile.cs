using AutoMapper;
using MongoShop.Areas.Admin.ViewModels.Category;
using MongoShop.Areas.Admin.ViewModels.Product;
using MongoShop.BusinessDomain.Categories;
using MongoShop.BusinessDomain.Products;
using MongoShop.BusinessDomain.Users;
using MongoShop.Models.Account;

namespace MongoShop
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, CreateProductViewModel>()
                .ForMember(dest=>dest.Category, opt=>opt.NullSubstitute(new Category()));

            CreateMap<CreateProductViewModel, Product>()
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(scr => scr.Category.Id))
                .ForMember(dest => dest.Category, opt => opt.Ignore());


            CreateMap<Product, DisplayProductViewModel>();
            CreateMap<Product, CreateProductViewModel>()
                .ForMember(dest=>dest.Images, opt=>opt.Ignore());

            CreateMap<CreateProductViewModel, DisplayProductViewModel>();

            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryViewModel, Category>();
        }
    }
}