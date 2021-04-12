namespace RealEstates.Services.Profiler
{
    using AutoMapper;
    using RealEstates.Models;
    using RealEstates.Services.Models;
    using System.Linq;

    public class RealEstatesProfiler : Profile
    {
        public RealEstatesProfiler()
        {
            this.CreateMap<Property, PropertyInfoDto>()
                .ForMember(x => x.BuildingType, y => y.MapFrom(s => s.BuildingType.Name));

            this.CreateMap<District, DistrictInfoDto>()
                .ForMember(x => x.AveragePricePerSquareMeter, 
                                          y => y.MapFrom(s => s.Properties
                                                .Where(x => x.Price.HasValue)
                                                .Average(p => p.Price / (decimal)p.Size) ?? 0));

            this.CreateMap<Property, PropertyInfoFullData>();      
        }
    }
}
