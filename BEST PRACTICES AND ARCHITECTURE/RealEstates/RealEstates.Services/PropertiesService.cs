namespace RealEstates.Services
{
    using System.Collections.Generic;


    using RealEstates.Data;
    using RealEstates.Services.Models;

    public class PropertiesService : IPropertiesService
    {
        private readonly ApplicationDbContext dbContext;

        public PropertiesService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(string district, int floor, int maxFloor, int size, int yardSize, int year, string propertyType, string buildingType, int price)
        {
            
        }

        public IEnumerable<PropertyInfoDto> Search(int minPrice, int maxPrice, int minSize, int maxSize)
        {
            return new List<PropertyInfoDto>();
        }
    }
}
