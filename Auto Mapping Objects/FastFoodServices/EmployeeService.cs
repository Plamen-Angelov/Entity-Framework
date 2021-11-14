using AutoMapper;
using AutoMapper.QueryableExtensions;
using FastFood.Data;
using FastFood.Models;
using FastFoodServices.DTO.Employee;
using FastFoodServices.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace FastFoodServices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper mapper;
        private readonly FastFoodContext dbContext;

        public EmployeeService(IMapper mapper, FastFoodContext context)
        {
            this.mapper = mapper;
            this.dbContext = context;
        }

        public void Register(RegisterEmployeeDTO dto)
        {
            Employee employee = mapper.Map<Employee>(dto);
            dbContext.Employees.Add(employee);
            dbContext.SaveChanges();
        }

        public ICollection<ListAllEmployeeDTO> All()
        => dbContext
            .Employees
            .ProjectTo<ListAllEmployeeDTO>(this.mapper.ConfigurationProvider)
            .ToList();
    }
}
