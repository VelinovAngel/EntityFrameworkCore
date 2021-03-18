using CarDealer.Data;
using CarDealer.DTO.InputModel;
using CarDealer.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var dbContext = new CarDealerContext();
            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();


            var xmlSuppliers = File.ReadAllText("Datasets/suppliers.xml");

            var result = ImportSuppliers(dbContext, xmlSuppliers);

            System.Console.WriteLine(result);
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(SupplierInputModel[]), new XmlRootAttribute("Suppliers"));

            var textReader = new StringReader(inputXml);

            var suppliersDto = (IEnumerable<SupplierInputModel>)xmlSerializer.Deserialize(textReader);

            var result = suppliersDto.Select(x => new Supplier
            {
                Name = x.Name,
                IsImporter = x.IsImporter
            });

            context.Suppliers.AddRange(result);

            context.SaveChanges();

            return $"Successfully imported {result.Count()}";
        }
    }
}