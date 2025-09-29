using API.DTOs.Products;
using API.Entities.Products;
using AutoMapper;

namespace API.AutoMapper;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductReturnDTO>();
    }
}