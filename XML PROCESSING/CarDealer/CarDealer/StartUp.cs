using System;
using System.IO;
using System.Linq;


using CarDealer.Data;
using CarDealer.Models;
using CarDealer.DTO.InputModel;
using System.Xml.Serialization;
using CarDealer.DTO.OutputModel;
using System.Collections.Generic;
using AutoMapper;

namespace CarDealer
{
    public class StartUp
    {
        static IMapper mapper;

        public static void Main(string[] args)
        {
            var dbContext = new CarDealerContext();
            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            //ar xmlFile = File.ReadAllText("Datasets/sales.xml");

            //var result = ImportSales(dbContext, xmlFile);

            Console.WriteLine(GetCarsWithTheirListOfParts(dbContext));
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsAndTheirParts = context.Cars
                .Select(x => new CarAndTheirPartsOutputModel
                {
                        Make = x.Make,
                        Model = x.Model,
                        TraveledDistance = x.TravelledDistance,
                            Parts = x.PartCars.Select(p => new PartsOutputModel
                            {
                                Name = p.Part.Name,
                                Price = p.Part.Price
                            })
                            .OrderByDescending(x=>x.Price)
                            .ToArray()
                          
                })
                .OrderByDescending(x => x.TraveledDistance)
                .ThenBy(x => x.Model)
                .Take(5)
                .ToArray();

            var stremWriter = new StringWriter();

            var cars = new XmlSerializer(typeof(CarAndTheirPartsOutputModel[]), new XmlRootAttribute("cars"));

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            cars.Serialize(stremWriter, carsAndTheirParts, ns);

            return stremWriter.ToString();
        }
        public static string GetLocalSuppliers(CarDealerContext context)
        {

            var suppliers = context.Suppliers
                .Where(x => x.IsImporter == false)
                .Select(x => new SuppliersOutputModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PartsCount = x.Parts.Count()
                })
                .ToArray();

            var streamWriter = new StringWriter();

            var suppliersSerialized = new XmlSerializer(typeof(SuppliersOutputModel[]), new XmlRootAttribute("suppliers"));

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            suppliersSerialized.Serialize(streamWriter, suppliers, ns);

            return streamWriter.ToString();
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var bmw = context.Cars
                .Where(x => x.Make == "BMW")
                .Select(x => new CarsBMWOutput
                {
                    Id = x.Id,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .ToArray();

            var streamWriter = new StringWriter();

            var carsSerializer = new XmlSerializer(typeof(CarsBMWOutput[]), new XmlRootAttribute("cars"));

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            carsSerializer.Serialize(streamWriter, bmw, ns);

            return streamWriter.ToString();
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(x => x.TravelledDistance >= 2000000)
                .OrderBy(x => x.Make)
                .ThenBy(x => x.Model)
                .Select(x => new CarOutputModel
                {
                    Make = x.Make,
                    Model = x.Model,
                    TraveledDistance = x.TravelledDistance
                })
                .Take(10)
                .ToArray();

            var carsSerialier = new XmlSerializer(typeof(CarOutputModel[]), new XmlRootAttribute("cars"));

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            var textWriter = new StringWriter();

            carsSerialier.Serialize(textWriter, cars, ns);

            return textWriter.ToString();
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(SalesInputModel[]), new XmlRootAttribute("Sales"));

            var textReader = new StringReader(inputXml);

            var dtoSales = (IEnumerable<SalesInputModel>)xmlSerializer.Deserialize(textReader);

            var carId = context.Cars.Select(x => x.Id).ToList();

            var result = dtoSales
                .Where(x => carId.Contains(x.CarId))
                .Select(x => new Sale
                {
                    CarId = x.CarId,
                    CustomerId = x.CustomerId,
                    Discount = x.Discount
                });

            context.AddRange(result);
            context.SaveChanges();

            return $"Successfully imported {result.Count()}";
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

        public static void InitializedAutomappe()
        {
            var config = new MapperConfiguration(cgf =>
             {
                 cgf.AddProfile<CarDealerProfile>();
             });

            mapper = config.CreateMapper();
        }
    }
}