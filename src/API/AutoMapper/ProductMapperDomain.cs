using API.DTOs;
using API.Entities.Products;

namespace API.AutoMapper;

public  class ProductMapperDomain
{
    public ProductReturnDTO ProductReturnDto { get; set; } = new ProductReturnDTO();
    public ProductMapperDomain(Product product)
    {
        ProductReturnDto.Id = product.Id;
        ProductReturnDto.Name = product.Name;
        ProductReturnDto.BrandId = product.Brand.Id;
        ProductReturnDto.Brand = new ProductBrandReturnDTO { Name = product.Brand.Name };
        ProductReturnDto.TypeId = product.TypeId;
        ProductReturnDto.Type = new ProductTypeReturnDTO { Name = product.Type.Name };;
        ProductReturnDto.Description = product.Description;
        ProductReturnDto.Price = product.Price;
        ProductReturnDto.QuantityInStock = product.QuantityInStock;
        ProductReturnDto.PictureUrl = product.PictureUrl;
    }
    // public static ProductReturnDTO MapperDomainToDto(this Product product)
    // {
    //     return new ProductReturnDTO
    //     {
    //         Id = product.Id,
    //         Name = product.Name,
    //         Description = product.Description,
    //         Price = product.Price,
    //         Brand = new ProductBrandReturnDTO { Name = product.Brand.Name },
    //         BrandId = product.BrandId,
    //         TypeId = product.TypeId,
    //         QuantityInStock = product.QuantityInStock,
    //     };
    // }
    
    public static implicit operator ProductMapperDomain(Product product) 
        => new ProductMapperDomain(product);
}