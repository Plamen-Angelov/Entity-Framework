
using FastFoodServices.DTO.Position;
using System.Collections.Generic;

namespace FastFoodServices.Interfaces
{
    public interface IPositionService
    {
        ICollection<EmployeeRegisterPositionsAvailable> GetPositionsAvailable();
    }
}
