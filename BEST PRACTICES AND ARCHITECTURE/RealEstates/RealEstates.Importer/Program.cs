namespace RealEstates.Importer
{
    using System;
    using System.IO;
    using System.Text.Json;
    using System.Collections.Generic;


    using RealEstates.Data;
    using RealEstates.Importer.ImportJsonModels;
    using RealEstates.Services;

    class Program
    {
        static void Main(string[] args)
        { 
            ImportJsonFile(@"..\..\..\DataSets\imot.bg-houses-Sofia-raw-data-2021-03-18.json");
            Console.WriteLine();
            ImportJsonFile(@"..\..\..\DataSets\imot.bg-raw-data-2021-03-18.json");
        }

        private static void ImportJsonFile(string jsonFileName)
        {
            var dbContext = new ApplicationDbContext();
            IPropertiesService propertiesService = new PropertiesService(dbContext);

            var properties = JsonSerializer.Deserialize<IEnumerable<PropertyAsJson>>(File.ReadAllText(jsonFileName));

            foreach (var propertyJson in properties)
            {
                propertiesService.Add(propertyJson.District, propertyJson.Floor, propertyJson.TotalFloors, propertyJson.Size, propertyJson.YardSize, propertyJson.Year, propertyJson.Type, propertyJson.BuildingType, propertyJson.Price);
                Console.Write(".");
            }
        }
    }
}
