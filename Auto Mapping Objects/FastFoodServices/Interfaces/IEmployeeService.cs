using FastFoodServices.DTO.Employee;
using System.Collections.Generic;

namespace FastFoodServices.Interfaces
{
    public interface IEmployeeService
    {
        void Register(RegisterEmployeeDTO dto);

        ICollection<ListAllEmployeeDTO> All();
    }
}
