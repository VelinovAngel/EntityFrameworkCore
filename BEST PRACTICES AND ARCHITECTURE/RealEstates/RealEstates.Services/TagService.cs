namespace RealEstates.Services
{
    using System;
    using System.Linq;
    using RealEstates.Data;
    using RealEstates.Models;

    public class TagService : ITagService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IPropertiesService propertiesService;

        public TagService(ApplicationDbContext dbContext, IPropertiesService propertiesService)
        {
            this.dbContext = dbContext;
            this.propertiesService = propertiesService;
        }
        public void Add(string name, int? importance = null)
        {
            var tag = new Tag
            {
                Name = name,
                Importance = importance
            };

            this.dbContext.Tags.Add(tag);
            this.dbContext.SaveChanges();
        }

        public void BulkTagToProperties()
        {
            var allProperties = dbContext.Properties
                .ToList();

            foreach (var property in allProperties)
            {
                var averagePriceForAnDistrict = this.propertiesService
                    .AveragePricePerSquareMeter(property.DistrictId);

                if (property.Price > averagePriceForAnDistrict)
                {
                    var currTag = dbContext.Tags.FirstOrDefault(x => x.Name == "скъп-имот");
                    property.Tags.Add(currTag);
                }

                if (property.Price < averagePriceForAnDistrict)
                {
                    var currTag = dbContext.Tags.FirstOrDefault(x => x.Name == "евтин-имот");
                    property.Tags.Add(currTag);
                }
            }

            dbContext.SaveChanges();
        }
    }
}
