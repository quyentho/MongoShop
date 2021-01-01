using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                .ForMember(dest => dest.SelectedCategoryId, opt => opt.NullSubstitute(new Category()))
                .ForPath(dest => dest.SelectedCategoryId, opt => opt.MapFrom(scr => scr.CategoryId))
                .ForPath(dest => dest.Images.Files, opt => opt.Ignore())
                .ForPath(dest => dest.Images.FilePaths, opt => opt.MapFrom(scr=>scr.Images));

            CreateMap<Product, IndexProductViewModel>()
                .ForPath(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<ProductViewModel, Product>()
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(scr => scr.SelectedCategoryId))
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryViewModel, Category>();

            CreateMap<Category, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(scr => scr.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));
        }
    }
}