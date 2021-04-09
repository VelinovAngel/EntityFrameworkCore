namespace RealEstates.ConsoleApplication
{
    using System;
    using System.Text;
    using Microsoft.EntityFrameworkCore;


    using RealEstates.Data;
    using RealEstates.Services;

    class Program
    {
        static void Main(string[] args)
        {
            var context = new ApplicationDbContext();
            context.Database.Migrate();

            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Property Search");
                Console.WriteLine("2. Most expensive districts");
                Console.WriteLine("3. Average price per square meter");
                Console.WriteLine("0. EXIT!");

                bool parsed = int.TryParse(Console.ReadLine(), out int option);

                if (parsed && option == 0)
                {
                    break;
                }

                if (parsed && option >= 1 && option <= 3)
                {
                    switch (option)
                    {
                        case 1:
                            PropertySearch(context);
                            break;
                        case 2:
                            MostExpensiveDistricts(context);
                            break;
                        case 3:
                            AveragePricePerSquareMeter(context);
                            break;
                        default:
                            break;
                    }

                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        private static void AveragePricePerSquareMeter(ApplicationDbContext context)
        {
            IPropertiesService service = new PropertiesService(context);
            var averagePricePerSquareMiters = service.AveragePricePerSquareMeter();
            
            Console.WriteLine($"Average price per square meters is : {averagePricePerSquareMiters:F2} €/m².");
        }

        private static void MostExpensiveDistricts(ApplicationDbContext context)
        {
            Console.Clear();
            Console.WriteLine("Most expensive districts:");
            Console.WriteLine("Insert districts count:");
            int count = int.Parse(Console.ReadLine());

            IDistrictsService districtService = new DistrictService(context);
            var districts = districtService.GetMostExpensiveDistricts(count);
            foreach (var district in districts)
            {
                Console.WriteLine($"{district.Name} => " +
                    $"Average price per square meter: {district.AveragePricePerSquareMeter:F2} €/m² .Count: ({district.PropertiesCount}) ");
            }
        }

        private static void PropertySearch(ApplicationDbContext context)
        {
            Console.Clear();
            Console.WriteLine("Property Search:");
            Console.Write("Min price:");
            int minPrice = int.Parse(Console.ReadLine());
            Console.Write("Max price:");
            int maxPrice = int.Parse(Console.ReadLine());
            Console.Write("Min size:");
            int minSize = int.Parse(Console.ReadLine());
            Console.Write("Max size:");
            int maxSize = int.Parse(Console.ReadLine());

            IPropertiesService service = new PropertiesService(context);
            var properties = service.Search(minPrice, maxPrice, minSize, maxSize);
            foreach (var property in properties)
            {
                Console.WriteLine($"{property.DistrictName} ; {property.BuildingType}; " +
                    $"{property.PropertyType} => {property.Price:F2}€ => {property.Size}m²");
            }
        }
    }
}
