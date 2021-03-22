using System;
using System.IO;
using System.Linq;
using AutoMapper;
using System.Xml.Serialization;


using ProductShop.Data;
using ProductShop.Models;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using System.Text;

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

            //Mapper.Initialize(cfg => cfg.AddProfile<ProductShopProfile>());

            //var fileXml = File.ReadAllText(@"Datasets\categories-products.xml");

            //var result = ImportCategoryProducts(context, fileXml);
            Console.WriteLine(GetUsersWithProducts(context));
        }
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var users = new UsersRootDto()
            {
                Count = context.Users.Count(u => u.ProductsSold.Any(p => p.Buyer != null)),
                Users = context.Users
                  .ToArray()
                  .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                  .OrderByDescending(u => u.ProductsSold.Count)
                  .Take(10)
                  .Select(u => new UserOutputModelDto()
                  {
                      FirstName = u.FirstName,
                      LastName = u.LastName,
                      Age = u.Age,
                      SoldProducts = new SoldProductDto()
                      {
                          Count = u.ProductsSold.Count(ps => ps.Buyer != null),
                          Products = u.ProductsSold
                              .ToArray()
                              .Where(ps => ps.Buyer != null)
                              .Select(ps => new ProductOutputModelDto()
                              {
                                  Name = ps.Name,
                                  Price = ps.Price
                              })
                              .OrderByDescending(p => p.Price)
                              .ToArray()
                      }
                  })

                  .ToArray()
            };

            var xmlSerializer = new XmlSerializer(typeof(UsersRootDto), new XmlRootAttribute("Users"));

            var stringWriter = new StringWriter();

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            xmlSerializer.Serialize(stringWriter, users, ns);

            return stringWriter.ToString();
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(x => new CategoriesOutputModel
                {
                    Name = x.Name,
                    Count = x.CategoryProducts.Count,
                    AveragePrice = x.CategoryProducts.Average(p => p.Product.Price),
                    TotalRevenue = x.CategoryProducts.Sum(p => p.Product.Price)

                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.TotalRevenue)
                .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(CategoriesOutputModel[]), new XmlRootAttribute("Categories"));

            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);

            var stringWriter = new StringWriter();

            xmlSerializer.Serialize(stringWriter, categories, ns);

            return stringWriter.ToString();
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