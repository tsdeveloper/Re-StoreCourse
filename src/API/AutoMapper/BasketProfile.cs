using API.DTOs.BasketItems;
using API.DTOs.Baskets;
using API.Entities.Baskets;
using AutoMapper;

namespace API.AutoMapper;

public class BasketProfile : Profile
{
    public BasketProfile()
    {
        CreateMap<Basket, BasketDTO>();
        CreateMap<BasketItem, BasketItemDto>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Product.Name))
            .ForMember(d => d.Price, o => o.MapFrom(s => s.Product.Price))
            .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl))
            .ForMember(d => d.Quantity, o => o.MapFrom(s => s.Quantity))
            ;
    }
}