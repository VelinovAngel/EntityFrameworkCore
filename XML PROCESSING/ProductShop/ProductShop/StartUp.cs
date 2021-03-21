using System;
using System.IO;
using AutoMapper;
using System.Xml.Serialization;


using ProductShop.Data;
using ProductShop.Dtos.Import;

namespace ProductShop
{
    public class StartUp
    {
        static IMapper mapper;
        public static void Main(string[] args)
        {
            var context = new ProductShopContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var fileXml = File.ReadAllText(@"Datasets\users.xml");

            var result = ImportUsers(context, fileXml);

            Console.WriteLine(result);
        }


        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(UserInputModel[]), new XmlRootAttribute("Users"));

            var usersDtos = xmlSerializer.Deserialize(new StringReader(inputXml));

            return $"Successfully imported {0}";
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