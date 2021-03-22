using AutoMapper;

using ProductShop.Models;
using ProductShop.Dtos.Import;
using ProductShop.Dtos.Export;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //Import Models
            this.CreateMap<UserInputModel, User>();

            this.CreateMap<ProductInputModel, Product>();

            this.CreateMap<CategorieInputModel, Category>();

            this.CreateMap<CategoriesProductsInputModel, CategoryProduct>();

            //Export Models

            this.CreateMap<Product, ProductOutputModel>();

            this.CreateMap<Category, CategoriesOutputModel>();
        }
    }
}
