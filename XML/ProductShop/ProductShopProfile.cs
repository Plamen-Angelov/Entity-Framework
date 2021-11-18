using AutoMapper;
using ProductShop.Dto.Input;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<ProductInputDto, Product>();
        }
    }
}
