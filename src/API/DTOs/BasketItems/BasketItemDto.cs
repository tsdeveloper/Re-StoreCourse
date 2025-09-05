using API.Entities.Baskets;

namespace API.DTOs.BasketItems;

public class BasketItemDto
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public int ProductId { get; set; }
    public int BasketId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string PictureUrl { get; set; }

    public static explicit operator BasketItemDto(BasketItem entity)
    {
        return new BasketItemDto
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            ProductId = entity.ProductId,
            BasketId = entity.BasketId,
            Name = entity.Product.Name,
            PictureUrl = entity.Product.PictureUrl,
            Price = entity.Product.Price,
        };
    }
}