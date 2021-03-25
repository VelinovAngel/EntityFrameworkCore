using System;
using System.IO;
using AutoMapper;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


using ProductShop.Data;
using ProductShop.DateDTO;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        static IMapper mapper;
        public static void Main(string[] args)
        {
            ProductShopContext context = new ProductShopContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //string inputJsonUser = File.ReadAllText("../../../Datasets/users.json");
            //string inputJsonProduct = File.ReadAllText("../../../Datasets/products.json");
            //string inputJsonCategories = File.ReadAllText("../../../Datasets/categories.json");
            //string inputJsonCategories = File.ReadAllText("../../../Datasets/categories-products.json");

            //var resultUsers = ImportUsers(context, inputJsonUser);
            //var resultProduct = ImportProducts(context, inputJsonProduct);
            //var resultCategories = ImportCategories(context, inputJsonCategories);
            //var resultCategoriesProducts = ImportCategoryProducts(context, inputJsonCategories);


            Console.WriteLine(GetUsersWithProducts(context));
            File.WriteAllText("../../../users-and-products.json", GetCategoriesByProductsCount(context));
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Include(i=>i.ProductsSold)
                .ToList()
                .Where(x => x.ProductsSold.Any(s => s.BuyerId != null))
                .Select(x => new
                {
                    firstName = x.FirstName,
                    lastName = x.LastName,
                    age = x.Age,
                    soldProducts = new
                    {
                        count = x.ProductsSold.Where(b => b.BuyerId != null).Count(),
                        products = x.ProductsSold.Where(b => b.BuyerId != null)
                                                 .Select(p => new
                                                 {
                                                     name = p.Name,
                                                     price = p.Price
                                                 })
                    }
                })
                .OrderByDescending(x => x.soldProducts.products.Count())
                .ToList();

            var resultObject = new
            {
                usersCount = users.Count(),
                users = users
            };

            var usersToJson = JsonConvert.SerializeObject(resultObject, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            return usersToJson;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(x => new
                {
                    category = x.Name,
                    productsCount = x.CategoryProducts.Count,
                    averagePrice = (x.CategoryProducts.Average(p => p.Product.Price)).ToString("f2"),
                    totalRevenue = x.CategoryProducts.Sum(s => s.Product.Price).ToString("f2")
                })
                .OrderByDescending(x => x.productsCount)
                .ToList();

            var categoriesToJson = JsonConvert.SerializeObject(categories, Formatting.Indented);

            return categoriesToJson;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(x => x.ProductsSold.Any(b => b.Buyer != null))
                .Where(x => x.ProductsSold.Count > 0)
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Select(x => new
                {
                    firstName = x.FirstName,
                    lastName = x.LastName,
                    soldProducts = x.ProductsSold.Select(p => new
                    {
                        name = p.Name,
                        price = p.Price,
                        buyerFirstName = p.Buyer.FirstName,
                        buyerLastName = p.Buyer.LastName
                    }
                    )
                })
                .ToList();

            var usersToJson = JsonConvert.SerializeObject(users, Formatting.Indented);

            return usersToJson;
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .Select(x => new
                {
                    name = x.Name,
                    price = x.Price,
                    seller = x.Seller.FirstName + " " + x.Seller.LastName
                })
                .OrderBy(x => x.price)
                .ToList();

            var productsToJson = JsonConvert.SerializeObject(products, Formatting.Indented);

            return productsToJson;
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var dtoCategoriesProducts = JsonConvert.DeserializeObject<IEnumerable<CategoriesAndProductsInputModel>>(inputJson);

            InitializeAutoMapper();

            var categoriesProducts = mapper.Map<IEnumerable<CategoryProduct>>(dtoCategoriesProducts);

            context.AddRange(categoriesProducts);

            context.SaveChanges();

            return $"Successfully imported {categoriesProducts.Count()}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            //Import the users from the provided file categories.json.Some of the names will be null, so you don’t have to add them in the database. Just skip the record and continue.
            var dtoCategories = JsonConvert.DeserializeObject<IEnumerable<CategoryInputModel>>(inputJson, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            InitializeAutoMapper();

            var categories = mapper.Map<IEnumerable<Category>>(dtoCategories);

            //int count = 0;
            //foreach (var category in categories)
            //{
            //    if (category.Name != null)
            //    {
            //        context.Add(category);
            //        count++;
            //    }
            //}

            context.SaveChanges();

            return $"Successfully imported {dtoCategories.Count()}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            //Import the users from the provided file products.json.
            //Your method should return string with message $"Successfully imported {Products.Count}";
            var dtoProducts = JsonConvert.DeserializeObject<IEnumerable<ProductInputModel>>(inputJson);

            InitializeAutoMapper();

            var products = mapper.Map<IEnumerable<Product>>(dtoProducts);

            context.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count()}";
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            //Import the users from the provided file users.json.
            //Your method should return string with message $"Successfully imported {Users.Count}";
            var dtoUsers = JsonConvert.DeserializeObject<IEnumerable<UserInputModel>>(inputJson);

            InitializeAutoMapper();

            var users = mapper.Map<IEnumerable<User>>(dtoUsers);

            context.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count()}";
        }

        private static void InitializeAutoMapper()
        {
            var configAutoMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
            mapper = configAutoMapper.CreateMapper();
        }
    }
}