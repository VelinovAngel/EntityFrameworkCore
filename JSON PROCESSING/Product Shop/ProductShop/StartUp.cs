using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
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
            string inputJsonCategories = File.ReadAllText("../../../Datasets/categories.json");

            //var resultUsers = ImportUsers(context, inputJsonUser);
            //var resultProduct = ImportProducts(context, inputJsonProduct);
            var resultCategories = ImportCategories(context, inputJsonCategories);

            Console.WriteLine(resultCategories);
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            //Import the users from the provided file categories.json.Some of the names will be null, so you don’t have to add them in the database. Just skip the record and continue.
            var dtoCategories = JsonConvert.DeserializeObject<IEnumerable<CategoryInputModel>>(inputJson);

            InitializeAutoMapper();

            var categories = mapper.Map<IEnumerable<Category>>(dtoCategories);

            int count = 0;
            foreach (var category in categories)
            {
                if (category.Name != null)
                {
                    context.Add(category);
                    count++;
                }
            }

            context.SaveChanges();

            return $"Successfully imported {count}";
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