using API.DTOs.BasketItems;
using API.Entities.Baskets;

namespace API.DTOs.Baskets;

public class BasketDTO
{
    public int Id { get; set; }
    public string BuyerId { get; set; }
    public List<BasketItemDto> BasketItems { get; set; } = new();

    public string PaymentIntendId { get; set; }

    public static explicit operator BasketDTO(Basket entity)
    {
        if (entity == null) return null;
        
        var dto = new  BasketDTO
        {
            Id = entity.Id,
            BuyerId = entity.BuyerId,
            PaymentIntendId = entity.PaymentIntendId,
           
        };
        dto.BasketItems.AddRange(entity.BasketItems.Select(x => (BasketItemDto)x).ToList());
        
        return dto;
    }
}
