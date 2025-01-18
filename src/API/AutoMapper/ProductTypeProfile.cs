using API.DTOs;
using API.Entities.Products;
using AutoMapper;

namespace API.AutoMapper;

public class ProductTypeProfile : Profile
{
  public ProductTypeProfile()
  {
    CreateMap<ProductType, ProductTypeReturnDTO>();
    
  }
}
