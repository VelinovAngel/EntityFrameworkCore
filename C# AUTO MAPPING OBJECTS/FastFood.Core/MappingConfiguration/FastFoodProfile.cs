namespace FastFood.Core.MappingConfiguration
{
    using AutoMapper;

    using FastFood.Models;
    using ViewModels.Positions;
    using FastFood.Core.ViewModels.Items;
    using FastFood.Core.ViewModels.Employees;
    using FastFood.Core.ViewModels.Categories;

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

        }
    }
}
