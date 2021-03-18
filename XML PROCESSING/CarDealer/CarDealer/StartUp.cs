using System;
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

            var xmlFile = File.ReadAllText("Datasets/parts.xml");

            var result = ImportParts(dbContext, xmlFile);

            Console.WriteLine(result);
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(CarInputModel[]), new XmlRootAttribute("Cars"));

            var textReader = new StringReader(inputXml);

            var suppliersDto = (IEnumerable<CarInputModel>)xmlSerializer.Deserialize(textReader);

            var result = suppliersDto.Select(x => new Car
            {
                Make = x.Make,
                Model = x.Model,
                TravelledDistance = x.TravelledDistance,

            });

            return $"Successfully imported {0}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(PartsInputModel[]), new XmlRootAttribute("Parts"));

            var textReader = new StringReader(inputXml);

            var suppliersDto = (IEnumerable<PartsInputModel>)xmlSerializer.Deserialize(textReader);

            var supplierId = context.Suppliers
                .Select(x => x.Id)
                .ToList();

            var result = suppliersDto
                .Where(s => supplierId.Contains(s.SupplierId))
                .Select(x => new Part
                {
                    Name = x.Name,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    SupplierId = x.SupplierId

                })
                .ToList();

            context.Parts.AddRange(result);

            context.SaveChanges();

            return $"Successfully imported {result.Count}";
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