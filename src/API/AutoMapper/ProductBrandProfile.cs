using API.DTOs;
using API.Entities.Products;
using AutoMapper;

namespace API.AutoMapper;

public class ProductBrandProfile : Profile
{
  public ProductBrandProfile()
  {
    CreateMap<ProductBrand, ProductBrandReturnDTO>();
  }
}
