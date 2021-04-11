namespace RealEstates.Services
{
    using System;
    using System.Linq;
    using RealEstates.Data;
    using RealEstates.Models;

    public class TagService : ITagService
    {
        private readonly ApplicationDbContext dbContext;

        public TagService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
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
                var averagePriceForAnDistrict = dbContext.Properties
                    .Where(x => x.DistrictId == property.DistrictId)
                    .Average(x => x.Price / (decimal));
            }
        }
    }
}
