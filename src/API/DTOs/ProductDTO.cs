using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.Products;

namespace API.DTOs;

public class ProductReturnDTO
{
    public int Id { get; set; }
    public int TypeId { get; set; }
    public int BrandId { get; set; }
    public ProductBrandReturnDTO Brand { get; set; }
    public ProductTypeReturnDTO Type { get; set; }
    public int QuantityInStock { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public long Price { get; set; }
    public string PictureUrl { get; set; }
}
