using System;
using System.IO;
using AutoMapper;
using System.Xml.Serialization;


using ProductShop.Data;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System.Collections;
using System.Linq;
using AutoMapper.QueryableExtensions;
using ProductShop.Dtos.Export;

namespace ProductShop
{
    public class StartUp
    {
        static IMapper mapper;
        public static void Main(string[] args)
        {
            var context = new ProductShopContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            Mapper.Initialize(cfg => cfg.AddProfile<ProductShopProfile>());

            //var fileXml = File.ReadAllText(@"Datasets\categories-products.xml");

            //var result = ImportCategoryProducts(context, fileXml);
            Console.WriteLine(GetSoldProducts(context));
        }
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {

        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            //Get all users who have at least 1 sold item. Order them by last name, then by first name. Select the person's first and last name. For each of the sold products, select the product's name and price. Take top 5 records.

            var users = context.Users
                .Where(x => x.ProductsSold.Any(b => b.Buyer != null))
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Take(5)
                .Select(x => new UsersOutputModel
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Product = x.ProductsSold
                    .Where(c => c.Buyer != null)
                    .Select(a => new ProductOutputModelDto
                    {
                        Name = a.Name,
                        Price = a.Price
                    }).ToArray()
                })
                .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(UsersOutputModel[]), new XmlRootAttribute("Users"));

            var streamWriter = new StringWriter();

            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);

            xmlSerializer.Serialize(streamWriter, users, ns);

            return streamWriter.ToString();
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            //Get all products in a specified price range between 500 and 1000(inclusive).Order them by price(from lowest to highest).Select only the product name, price and the full name of the buyer.Take top 10 records.

            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .OrderBy(x => x.Price)
                .Select(x => new ProductOutputModel
                {
                    Name = x.Name,
                    Price = x.Price,
                    Buyer = x.Buyer.FirstName + " " + x.Buyer.LastName
                })
                .Take(10)
                .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(ProductOutputModel[]), new XmlRootAttribute("Products"));

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            var streamWriter = new StringWriter();

            xmlSerializer.Serialize(streamWriter, products, ns);

            return streamWriter.ToString();
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(CategoriesProductsInputModel[]), new XmlRootAttribute("CategoryProducts"));

            //InizializedAutomapper();

            var categoriesProducts = xmlSerializer.Deserialize(new StringReader(inputXml));

            var categoriesProductsDto = Mapper.Map<CategoryProduct[]>(categoriesProducts);

            context.CategoryProducts.AddRange(categoriesProductsDto);
            context.SaveChanges();

            return $"Successfully imported {categoriesProductsDto.Length}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
           // InizializedAutomapper();

            var xmlSerializer = new XmlSerializer(typeof(CategorieInputModel[]), new XmlRootAttribute("Categories"));

            var categories = xmlSerializer.Deserialize(new StringReader(inputXml));

            var categoryDto = Mapper.Map<Category[]>(categories).Where(x => x.Name != null).ToArray();

            context.Categories.AddRange(categoryDto);
            context.SaveChanges();

            return $"Successfully imported {categoryDto.Length}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(ProductInputModel[]), new XmlRootAttribute("Products"));

            var products = xmlSerializer.Deserialize(new StringReader(inputXml));

            //InizializedAutomapper();

            var productsDto = Mapper.Map<Product[]>(products);

            context.Products.AddRange(productsDto);
            context.SaveChanges();

            return $"Successfully imported {productsDto.Length}";
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(UserInputModel[]), new XmlRootAttribute("Users"));

            var usersDtos = xmlSerializer.Deserialize(new StringReader(inputXml));

            //InizializedAutomapper();

            var usersMap = Mapper.Map<User[]>(usersDtos);

            context.Users.AddRange(usersMap);
            context.SaveChanges();

            return $"Successfully imported {usersMap.Length}";
        }

        public static void InizializedAutomapper()
        {
            //var automapper = new MapperConfiguration(cgf =>
            //{
            //    cgf.AddProfile<ProductShopProfile>();
            //});
            //mapper = automapper.CreateMapper();
        }
    }
}