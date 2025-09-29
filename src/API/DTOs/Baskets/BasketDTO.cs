using API.DTOs.BasketItems;
using API.Entities.Baskets;

namespace API.DTOs.Baskets;

public class BasketDTO
{
    public int Id { get; set; }
    public string BuyerId { get; set; }
    public List<BasketItemDto> BasketItems { get; set; } = new();

    public string PaymentIntentId { get; set; }
    public string ClientSecret { get; set; }


    public static explicit operator BasketDTO(Basket entity)
    {
        if (entity == null) return null;

        var dto = new BasketDTO
        {
            Id = entity.Id,
            BuyerId = entity.BuyerId,
            PaymentIntentId = entity.PaymentIntentId,
            ClientSecret = entity.ClientSecret,
        };
        dto.BasketItems.AddRange(entity.BasketItems.Select(x => (BasketItemDto)x).ToList());

        return dto;
    }
}