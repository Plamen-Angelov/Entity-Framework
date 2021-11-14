

using AutoMapper;
using AutoMapper.QueryableExtensions;
using FastFood.Data;
using FastFoodServices.DTO.Position;
using FastFoodServices.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace FastFoodServices
{
    public class PositionService : IPositionService
    {
        private readonly FastFoodContext dbContext;
        private readonly IMapper mapper;

        public PositionService(FastFoodContext context, IMapper mapper)
        {
            this.dbContext = context;
            this.mapper = mapper;
        }

        public ICollection<EmployeeRegisterPositionsAvailable> GetPositionsAvailable()
        => this.dbContext
            .Positions
            .ProjectTo<EmployeeRegisterPositionsAvailable>(this.mapper.ConfigurationProvider)
            .ToList();
    }
}
