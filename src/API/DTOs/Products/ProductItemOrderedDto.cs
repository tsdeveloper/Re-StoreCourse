using API.Entities.Aggregate;

namespace API.DTOs.Products;

public class ProductItemOrderedDto
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string PictureUrl { get; set; }

    public static explicit operator ProductItemOrderedDto(ProductItemOrdered dto)
    {
        return new ProductItemOrderedDto
        {
            ProductId = dto.ProductId,
            Name = dto.Name,
            PictureUrl = dto.PictureUrl
        };
    }
}