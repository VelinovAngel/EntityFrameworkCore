using AutoMapper;
using CarDealer.DTO.OutputModel;
using CarDealer.Models;
using System;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<SuppliersOutputModel, Supplier>();
        }
    }
}
