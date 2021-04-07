namespace RealEstates.Services
{
    using System.Collections.Generic;

    using RealEstates.Data;
    using RealEstates.Services.Models;

    public class DistrictService : IDistrictsService
    {
        private readonly ApplicationDbContext context;

        public DistrictService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<DistrictInfoDto> GetMostExpensiveDistricts(int count)
        {
            return new List<DistrictInfoDto>();
        }
    }
}
