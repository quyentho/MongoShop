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
            CreateMap<Product, ProductViewModel>()
                .ForMember(dest => dest.Category, opt => opt.NullSubstitute(new Category()))
                .ForPath(dest => dest.Category.Id, opt => opt.MapFrom(scr => scr.CategoryId))
                .ForPath(dest => dest.Images.Files, opt => opt.Ignore())
                .ForPath(dest => dest.Images.FilePaths, opt => opt.MapFrom(scr=>scr.Images));


            CreateMap<ProductViewModel, Product>()
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(scr => scr.Category.Id))
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryViewModel, Category>();
        }
    }
}