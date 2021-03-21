using System;
using System.IO;
using AutoMapper;
using System.Xml.Serialization;


using ProductShop.Data;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System.Collections;
using System.Linq;

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

            var fileXml = File.ReadAllText(@"Datasets\categories-products.xml");
  
            var result = ImportCategoryProducts(context, fileXml);
            Console.WriteLine(result);
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var xmlSerialier = new XmlSerializer(typeof(CategoriesProductsInputModel[]), new XmlRootAttribute("CategoryProducts"));

            InizializedAutomapper();

            var categoriesProducts = xmlSerialier.Deserialize(new StringReader(inputXml));

            var categoriesProductsDto = mapper.Map<CategoryProduct[]>(categoriesProducts);

            context.CategoryProducts.AddRange(categoriesProductsDto);
            context.SaveChanges();

            return $"Successfully imported {categoriesProductsDto.Length}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            InizializedAutomapper();

            var xmlSerializer = new XmlSerializer(typeof(CategorieInputModel[]), new XmlRootAttribute("Categories"));

            var categories = xmlSerializer.Deserialize(new StringReader(inputXml));

            var categoryDto = mapper.Map<Category[]>(categories).Where(x => x.Name != null).ToArray();

            context.Categories.AddRange(categoryDto);
            context.SaveChanges();

            return $"Successfully imported {categoryDto.Length}";
        }
        
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(ProductInputModel[]), new XmlRootAttribute("Products"));

            var products = xmlSerializer.Deserialize(new StringReader(inputXml));

            InizializedAutomapper();

            var productsDto = mapper.Map<Product[]>(products);

            context.Products.AddRange(productsDto);
            context.SaveChanges();

            return $"Successfully imported {productsDto.Length}";
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(UserInputModel[]), new XmlRootAttribute("Users"));

            var usersDtos = xmlSerializer.Deserialize(new StringReader(inputXml));

            InizializedAutomapper();

            var usersMap = mapper.Map<User[]>(usersDtos);

            context.Users.AddRange(usersMap);
            context.SaveChanges();

            return $"Successfully imported {usersMap.Length}";
        }

        public static void InizializedAutomapper()
        {
            var automapper = new MapperConfiguration(cgf =>
            {
               cgf.AddProfile<ProductShopProfile>();
            });

            mapper = automapper.CreateMapper();
        }
    }
}