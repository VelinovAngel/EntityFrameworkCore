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

            var xmlFile = File.ReadAllText("Datasets/customers.xml");

            var result = ImportCustomers(dbContext, xmlFile);

            Console.WriteLine(result);
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(CustomersInputModel[]), new XmlRootAttribute("Customers"));

            var textReader = new StringReader(inputXml);

            var dtoCustomers = (IEnumerable<CustomersInputModel>)xmlSerializer.Deserialize(textReader);

            var result = dtoCustomers.Select(x => new Customer
            {
                Name = x.Name,
                BirthDate = x.BirthDate,
                IsYoungDriver = x.IsYoungDriver
            })
                .ToList();

            context.AddRange(result);
            context.SaveChanges();

            return $"Successfully imported {result.Count}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(CarInputModel[]), new XmlRootAttribute("Cars"));

            var textReader = new StringReader(inputXml);

            var suppliersDto = (IEnumerable<CarInputModel>)xmlSerializer.Deserialize(textReader);

            var cars = new List<Car>();
            var carParts = new List<PartCar>();

            var allParts = context.Parts.Select(x => x.Id).ToList();

            foreach (var dtoCar in suppliersDto)
            {
                var distinctedParts = dtoCar.CarPartsInputModel.Select(x => x.Id).Distinct();
                var parts = distinctedParts.Intersect(allParts);

                var car = new Car()
                {
                    Make = dtoCar.Make,
                    Model = dtoCar.Model,
                    TravelledDistance = dtoCar.TraveledDistance
                };

                foreach (var part in parts)
                {
                    var carPart = new PartCar()
                    {
                        PartId = part,
                        Car = car
                    };  
                    carParts.Add(carPart);
                }
                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.PartCars.AddRange(carParts);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
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