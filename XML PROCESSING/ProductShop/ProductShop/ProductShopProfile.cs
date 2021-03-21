﻿using AutoMapper;

using ProductShop.Models;
using ProductShop.Dtos.Import;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<UserInputModel, User>();

            this.CreateMap<ProductInputModel, Product>();

            this.CreateMap<CategorieInputModel, Category>();

            this.CreateMap<CategoriesProductsInputModel, CategoryProduct>();
        }
    }
}
