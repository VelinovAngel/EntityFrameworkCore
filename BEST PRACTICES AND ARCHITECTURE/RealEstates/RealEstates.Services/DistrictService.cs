namespace RealEstates.Services
{
    using System.Linq;
    using System.Collections.Generic;
    using AutoMapper.QueryableExtensions;

    using RealEstates.Data;
    using RealEstates.Services.Models;

    public class DistrictService : BaseService , IDistrictsService
    {
        private readonly ApplicationDbContext context;

        public DistrictService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<DistrictInfoDto> GetMostExpensiveDistricts(int count)
        {
            var districts = context.Districts
                .ProjectTo<DistrictInfoDto>(this.Mapper.ConfigurationProvider)
                .ToList();
                //.Select(x => new DistrictInfoDto
                //{
                //    Name = x.Name,
                //    PropertiesCount = x.Properties.Count(),
                //    AveragePricePerSquareMeter = x.Properties
                //                                        .Where(x => x.Price.HasValue)
                //                                        .Average(p => p.Price / (decimal)p.Size) ?? 0,
                //})
                //.OrderByDescending(x=>x.AveragePricePerSquareMeter)
                //.Take(count)
                //.ToList();

            return districts;
        }
    }
}
