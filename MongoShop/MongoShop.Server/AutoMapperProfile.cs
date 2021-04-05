﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoShop.Areas.Admin.ViewModels.Product;
using MongoShop.BusinessDomain.Categories;
using MongoShop.BusinessDomain.Products;
using MongoShop.Server.ViewModels.Category;

namespace MongoShop.Server
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

            CreateMap<Product, ProductViewModel>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<Product, EditProductViewModel>()
               .ForMember(dest => dest.Category, opt => opt.Ignore());

            #endregion

            #region Category
            CreateMap<Category, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));

            CreateMap<Category, CategoryViewModel>();
            CreateMap<Category, EditCategoryViewModel>();
            CreateMap<EditCategoryViewModel, Category>();
            CreateMap<CreateCategoryViewModel, Category>();

            #endregion

            #region Order
            //CreateMap<Order, IndexOrderViewModel>();

            //CreateMap<CartCheckoutViewModel, Order>()
            //   .ForPath(dest => dest.ShipAddress.Street, opt => opt.MapFrom(scr => scr.Street))
            //   .ForPath(dest => dest.ShipAddress.Number, opt => opt.MapFrom(scr => scr.AddressNumber))
            //   .ForPath(dest => dest.ShipAddress.City, opt => opt.MapFrom(scr => scr.City));

            #endregion

            #region Cart
            //CreateMap<Cart, CartIndexViewModel>();
            //CreateMap<Cart, CartCheckoutViewModel>();
            //CreateMap<CartIndexViewModel, CartCheckoutViewModel>();
            #endregion

            #region Wish list
            //CreateMap<Wishlist, WishlistIndexViewModel>();
            #endregion

            #region User
            //CreateMap<ApplicationUser, AccountProfileViewModel>()
            //     .ForMember(dest => dest.AddressNumber, opt => opt.MapFrom(src => src.Contact.Address.Number))
            //     .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Contact.Address.Street))
            //     .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Contact.Address.City))
            //     .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Contact.PhoneNumber));

            //CreateMap<AccountProfileViewModel, ApplicationUser>(MemberList.Source)
            //     .ForPath(dest => dest.Contact.Address.Number, opt => opt.MapFrom(src => src.AddressNumber))
            //     .ForPath(dest => dest.Contact.Address.Street, opt => opt.MapFrom(src => src.Street))
            //     .ForPath(dest => dest.Contact.Address.City, opt => opt.MapFrom(src => src.City))
            //     .ForPath(dest => dest.Contact.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));
            #endregion
        }
    }
}