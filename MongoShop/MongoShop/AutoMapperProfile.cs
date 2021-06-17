using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoShop.Areas.Admin.ViewModels.Category;
using MongoShop.Areas.Admin.ViewModels.Order;
using MongoShop.Areas.Admin.ViewModels.Product;
using MongoShop.BusinessDomain.Carts;
using MongoShop.BusinessDomain.Categories;
using MongoShop.BusinessDomain.Orders;
using MongoShop.BusinessDomain.Products;
using MongoShop.BusinessDomain.Users;
using MongoShop.BusinessDomain.Wishlists;
using MongoShop.Models.Account;
using MongoShop.Models.Cart;
using MongoShop.Models.Customer;
using MongoShop.Models.Wishlist;

namespace MongoShop
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Product
            CreateMap<Product, CreateProductViewModel>();


            CreateMap<CreateProductViewModel, Product>()
               .ForMember(dest => dest.Images, opt => opt.Ignore())
               .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<Product, IndexProductViewModel>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.SubCategory, opt => opt.MapFrom(src => src.SubCategory.Name));

            CreateMap<Product, MainPageProductList>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Images[0]))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category.Id))
                .ForMember(dest => dest.SubCategory, opt => opt.MapFrom(src => src.SubCategory.Name));

            CreateMap<Product, IndexViewModel>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<Product, DetailProductViewModel>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<Product, CustomerProductDetailViewModel>()
               .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<Product, EditProductViewModel>()
                .ForMember(dest => dest.SelectedCategoryId, opt => opt.MapFrom(src => src.Category.Id));

            CreateMap<EditProductViewModel, Product>()
                .ForMember(dest => dest.Images, opt => opt.Ignore());

            CreateMap<Product, IndexViewModel>()
                   .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name)); ;
            CreateMap<Product, DetailViewModel>()
                    .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));
            #endregion

            #region Category
            CreateMap<Category, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));

            CreateMap<Category, IndexCategoryViewModel>();
            CreateMap<Category, DetailCategoryViewModel>();
            CreateMap<Category, EditCategoryViewModel>();
            CreateMap<EditCategoryViewModel, Category>();
            CreateMap<CreateCategoryViewModel, Category>();

            #endregion

            #region Order
            CreateMap<Order, IndexOrderViewModel>();

            CreateMap<CartCheckoutViewModel, Order>()
               .ForPath(dest => dest.ShipAddress.Street, opt => opt.MapFrom(scr => scr.Street))
               .ForPath(dest => dest.ShipAddress.Number, opt => opt.MapFrom(scr => scr.AddressNumber))
               .ForPath(dest => dest.ShipAddress.City, opt => opt.MapFrom(scr => scr.City));

            #endregion

            #region Cart
            CreateMap<Cart, CartIndexViewModel>();
            CreateMap<Cart, CartCheckoutViewModel>();
            CreateMap<CartIndexViewModel, CartCheckoutViewModel>();
            CreateMap<CartCheckoutViewModel, Cart>();
            #endregion

            #region Wish list
            CreateMap<Wishlist, WishlistIndexViewModel>();
            #endregion

            #region User
            CreateMap<ApplicationUser, UpdateInformationViewModel>()
                 .ForMember(dest => dest.AddressNumber, opt => opt.MapFrom(src => src.Contact.Address.Number))
                 .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Contact.Address.Street))
                 .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Contact.Address.City))
                 .ForMember(dest => dest.BirthDay, opt => opt.MapFrom(src => src.BirthDay))
                 .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Contact.PhoneNumber));

            CreateMap<UpdateInformationViewModel, ApplicationUser>(MemberList.Source)
                 .ForPath(dest => dest.Contact.Address.Number, opt => opt.MapFrom(src => src.AddressNumber))
                 .ForPath(dest => dest.Contact.Address.Street, opt => opt.MapFrom(src => src.Street))
                 .ForPath(dest => dest.Contact.Address.City, opt => opt.MapFrom(src => src.City))
                 .ForPath(dest => dest.BirthDay, opt => opt.MapFrom(src => src.BirthDay))
                 .ForPath(dest => dest.Contact.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));
            #endregion
        }
    }
}