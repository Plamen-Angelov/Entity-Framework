namespace FastFood.Core.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Data;
    using FastFood.Models;
    using FastFoodServices.DTO.Employee;
    using FastFoodServices.DTO.Position;
    using FastFoodServices.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Employees;

    public class EmployeesController : Controller
    {
        private readonly IPositionService positionService;
        private readonly IEmployeeService employeeService;
        private readonly IMapper mapper;

        public EmployeesController(IPositionService positionSerice, IMapper mapper, IEmployeeService employeeService)
        {
            this.positionService = positionSerice;
            this.mapper = mapper;
            this.employeeService = employeeService;
        }

        public IActionResult Register()
        {
            ICollection<EmployeeRegisterPositionsAvailable> positionsDTO = this.positionService.GetPositionsAvailable();

            List<RegisterEmployeeViewModel> regViewModel = this.mapper
                .Map<ICollection<EmployeeRegisterPositionsAvailable>, ICollection<RegisterEmployeeViewModel>>(positionsDTO)
                .ToList();

            return this.View(regViewModel);
        }

        [HttpPost]
        public IActionResult Register(RegisterEmployeeInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.RedirectToAction("Register");
            }

            RegisterEmployeeDTO employee = mapper.Map<RegisterEmployeeDTO>(model);
            employeeService.Register(employee);

            return this.RedirectToAction("All");
        }

        public IActionResult All()
        {
            ICollection<ListAllEmployeeDTO> employeesDTOs = this.employeeService.All();

            List<EmployeesAllViewModel> employeesViewModels = this.mapper
                .Map<ICollection<ListAllEmployeeDTO>, ICollection<EmployeesAllViewModel>>(employeesDTOs)
                .ToList();

            return this.View(employeesViewModels);
        }
    }
}
