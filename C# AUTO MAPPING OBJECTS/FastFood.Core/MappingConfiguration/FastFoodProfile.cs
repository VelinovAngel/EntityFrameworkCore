namespace FastFood.Core.MappingConfiguration
{
    using AutoMapper;

    using FastFood.Models;
    using ViewModels.Positions;
    using FastFood.Core.ViewModels.Items;
    using FastFood.Core.ViewModels.Employees;
    using FastFood.Core.ViewModels.Categories;
    using FastFood.Core.ViewModels.Orders;
    using System;

    public class FastFoodProfile : Profile
    {
        public FastFoodProfile()
        {
            //Positions
            this.CreateMap<CreatePositionInputModel, Position>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.PositionName));

            this.CreateMap<Position, PositionsAllViewModel>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.Name));

            this.CreateMap<Position, RegisterEmployeeViewModel>()
                .ForMember(x => x.PositionId, y => y.MapFrom(s => s.Id));

            //Employees
            this.CreateMap<RegisterEmployeeInputModel, Employee>();

            this.CreateMap<Employee, RegisterEmployeeViewModel>();

            this.CreateMap<Employee, EmployeesAllViewModel>()
                .ForMember(x => x.Position, y => y.MapFrom(s => s.Position.Name));

            //Category
            this.CreateMap<Category, CategoryAllViewModel>();

            //Item
            this.CreateMap<Category, CreateItemViewModel>()
                .ForMember(x => x.CategoryId, y => y.MapFrom(s => s.Id));

            this.CreateMap<Item, ItemsAllViewModels>()
            .ForMember(x => x.Category, y => y.MapFrom(s => s.Category.Name));

            this.CreateMap<CreateItemInputModel, Item>();

            //Orders
            this.CreateMap<CreateOrderInputModel, Order>()
                .ForMember(x => x.DateTime, y => y.MapFrom(s => DateTime.Now));
            //    .ForMember(x => x.OrderItems, y => y.MapFrom(s => s.ItemId));

            this.CreateMap<Order, OrderAllViewModel>()
                .ForMember(x => x.Employee, y => y.MapFrom(s => s.Employee.Name))
                .ForMember(x => x.OrderId, y => y.MapFrom(s => s.Id))
                .ForMember(x => x.DateTime, y => y.MapFrom(s => s.DateTime.ToString("d")));
        }
    }
}
