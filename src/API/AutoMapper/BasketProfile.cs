using API.DTOs;
using API.Entities.Baskets;
using API.Entities.Products;
using AutoMapper;

namespace API.AutoMapper;

public class BasketProfile : Profile
{
  public BasketProfile()
  {
    CreateMap<Basket, BasketReturnDTO>();
    CreateMap<BasketItem, BasketItemReturnDto>();
  }
}
