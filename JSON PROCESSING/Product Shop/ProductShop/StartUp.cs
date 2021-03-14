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

            string inputJsonUser = File.ReadAllText("../../../Datasets/users.json");
            string inputJsonProduct = File.ReadAllText("../../../Datasets/products.json");

            //var resultUsers = ImportUsers(context, inputJsonUser);
            var resultProduct = ImportProducts(context, inputJsonProduct);
            Console.WriteLine(resultProduct);
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