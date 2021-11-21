using AutoMapper;
using CarDealer.Dto.Input;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            CreateMap<SuppliersInputDto, Supplier>();

            CreateMap<PartInputDto, Part>();

            CreateMap<CustomerInputDto, Customer>();
        }
    }
}
