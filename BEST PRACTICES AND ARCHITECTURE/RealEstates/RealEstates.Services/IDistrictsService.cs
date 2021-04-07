namespace RealEstates.Services
{
    using RealEstates.Services.Models;
    using System.Collections.Generic;

    public interface IDistrictsService
    {
        IEnumerable<DistrictInfoDto> GetMostExpensiveDistricts(int count);
    }
}
