namespace RealEstates.Services
{
    using System;
    using System.Linq;
    using RealEstates.Data;
    using RealEstates.Models;

    public class TagService : ITagService
    {
        private const int YEAR_PROPERTY = 15;

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

                if (property.Price >= averagePriceForAnDistrict)
                {
                    var currTag = GetTag("скъп-имот");
                    property.Tags.Add(currTag);
                }
                else if (property.Price < averagePriceForAnDistrict)
                {
                    var currTag = GetTag("евтин-имот");
                    property.Tags.Add(currTag);
                }

                var currDate = DateTime.Now.AddYears(YEAR_PROPERTY);

                if (property.Year.HasValue && property.Year <= currDate.Year)
                {
                    var currTag = GetTag("стар-имот");
                    property.Tags.Add(currTag);
                }
                else if (property.Year.HasValue && property.Year > currDate.Year)
                {
                    var currTag = GetTag("нов-имот");
                    property.Tags.Add(currTag);
                }
            }
            dbContext.SaveChanges();
        }

        private Tag GetTag(string tag)
        {
            var currTag = dbContext.Tags.FirstOrDefault(x => x.Name == tag);

            return currTag;
        }
    }
}
