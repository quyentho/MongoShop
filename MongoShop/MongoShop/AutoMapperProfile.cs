﻿using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            #region Product
            CreateMap<Product, CreateProductViewModel>();

            CreateMap<CreateProductViewModel, Product>()
               .ForMember(dest => dest.Images, opt => opt.Ignore())
               .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(scr => scr.SelectedCategoryId))
               .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<Product, IndexProductViewModel>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<Product, DetailProductViewModel>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<Product, EditProductViewModel>()
                .ForMember(dest => dest.SelectedCategoryId, opt => opt.MapFrom(src => src.Category.Id));

            CreateMap<EditProductViewModel, Product>()
                .ForMember(dest=>dest.CategoryId, opt=>opt.MapFrom(src => src.SelectedCategoryId))
                .ForMember(dest => dest.Images, opt => opt.Ignore());
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
        }
    }
}