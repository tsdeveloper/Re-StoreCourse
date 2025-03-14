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
    CreateMap<BasketItem, BasketItemReturnDto>()
      .ForMember(d => d.Name, o => o.MapFrom(s => s.Product.Name))
      .ForMember(d => d.Price, o => o.MapFrom(s => s.Product.Price))
      .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl))
      .ForMember(d => d.Quantity, o => o.MapFrom(s => s.Quantity))
      ;
  }
}
