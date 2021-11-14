namespace FastFood.Core.MappingConfiguration
{
    using AutoMapper;
    using FastFood.Core.ViewModels.Categories;
    using FastFood.Core.ViewModels.Employees;
    using FastFood.Models;
    using FastFoodServices.DTO.Category;
    using FastFoodServices.DTO.Employee;
    using FastFoodServices.DTO.Position;
    using ViewModels.Positions;

    public class FastFoodProfile : Profile
    {
        public FastFoodProfile()
        {
            //Positions
            this.CreateMap<CreatePositionInputModel, Position>()
                .ForMember(d => d.Name, y => y.MapFrom(s => s.PositionName));

            this.CreateMap<Position, PositionsAllViewModel>()
                .ForMember(d => d.Name, y => y.MapFrom(s => s.Name));

            this.CreateMap<Position, EmployeeRegisterPositionsAvailable>()
                .ForMember(d => d.PositionId, y => y.MapFrom(s => s.Id))
                .ForMember(d => d.PisitionName, y => y.MapFrom(s => s.Name));


            //Categories
            
            this.CreateMap<CreateCategoryInputModel, CreateCategoryDTO>();

            this.CreateMap<ListAllCategoriesDTO, CategoryAllViewModel>()
                .ForMember(d => d.Name, s => s.MapFrom(s => s.CategoryName));

            this.CreateMap<CreateCategoryDTO, Category>()
                .ForMember(d => d.Name, s => s.MapFrom(x => x.CategoryName));

            this.CreateMap<Category, ListAllCategoriesDTO>()
                .ForMember(d => d.CategoryName, s => s.MapFrom(s => s.Name));

            //Employees

            CreateMap<EmployeeRegisterPositionsAvailable, RegisterEmployeeViewModel>()
                .ForMember(d => d.PositionName, y => y.MapFrom(s => s.PisitionName));

            CreateMap<RegisterEmployeeInputModel, RegisterEmployeeDTO>();

            CreateMap<RegisterEmployeeDTO, Employee>();

            CreateMap<ListAllEmployeeDTO, EmployeesAllViewModel>();

            CreateMap<Employee, ListAllEmployeeDTO>()
                .ForMember(d => d.Position, y => y.MapFrom(s => s.Position.Name));
        }
    }
}
