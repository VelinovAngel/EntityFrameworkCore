using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        static IMapper mapper;
        public static void Main(string[] args)
        {
            CarDealerContext context = new CarDealerContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //string inputSuppliersFromJson = File.ReadAllText("../../../Datasets/suppliers.json");
            //string inputPartsFromJson = File.ReadAllText("../../../Datasets/parts.json");
            //string inputCarsFromJson = File.ReadAllText("../../../Datasets/cars.json");
            //string inputCustomersFromJson = File.ReadAllText("../../../Datasets/customers.json");
            //string inputSalesFromJson = File.ReadAllText("../../../Datasets/sales.json");

            var resultSales = GetOrderedCustomers(context);


            Console.WriteLine(resultSales);
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .OrderBy(x => x.BirthDate)
                .ThenBy(x => x.IsYoungDriver)
                .Select(x => new
                {
                    Name = x.Name,
                    BirthDate = x.BirthDate.ToString("dd/MM/yyyy"),
                    IsYoungDriver = x.IsYoungDriver
                })
                .ToList();

            var result = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return result;
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var dtoSales = JsonConvert.DeserializeObject<IEnumerable<SalesInputModel>>(inputJson);

            InitializerAutoMapper();

            var sales = mapper.Map<IEnumerable<Sale>>(dtoSales);

            context.AddRange(sales);

            context.SaveChanges();

            return $"Successfully imported {sales.Count()}.";
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var dtoCustomers = JsonConvert.DeserializeObject<IEnumerable<CustomersInputModel>>(inputJson);

            InitializerAutoMapper();

            var customers = mapper.Map<IEnumerable<Customer>>(dtoCustomers);


            context.AddRange(customers);
            context.SaveChanges();


            return $"Successfully imported {customers.Count()}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var dtoCars = JsonConvert.DeserializeObject<IEnumerable<CarsInputModel>>(inputJson);

            var cars = new List<Car>();
            var carParts = new List<PartCar>();

            foreach (var dtoCar in dtoCars)
            {

                var car = new Car()
                {
                    Make = dtoCar.Make,
                    Model = dtoCar.Model,
                    TravelledDistance = dtoCar.TravelledDistance
                };

                foreach (var part in dtoCar.PartsId.Distinct())
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

            return $"Successfully imported {cars.Count()}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var suppliersId = context.Suppliers
                .Select(x => x.Id)
                .ToList();

            var dtoParts = JsonConvert.DeserializeObject<IEnumerable<PartsInputModel>>(inputJson)
                .Where(x => suppliersId.Contains(x.SupplierId))
                .ToList();

            InitializerAutoMapper();

            var parts = mapper.Map<IEnumerable<Part>>(dtoParts);

            context.AddRange(parts);

            context.SaveChanges();

            return $"Successfully imported {parts.Count()}.";
        }
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var dtoSupplieres = JsonConvert.DeserializeObject<IEnumerable<SuppliersInputModel>>(inputJson);

            InitializerAutoMapper();

            var suppliers = mapper.Map<IEnumerable<Supplier>>(dtoSupplieres);

            context.AddRange(suppliers);
            context.SaveChanges();


            return $"Successfully imported {suppliers.Count()}.";
        }

        public static void InitializerAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            mapper = config.CreateMapper();
        }
    }
}