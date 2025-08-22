using API.DTOs.BasketItems;
using API.Entities.Baskets;

namespace API.DTOs.Baskets;

public class BasketReturnDTO
{
    public int Id { get; set; }
    public string BuyerId { get; set; }
    public List<BasketItemReturnDto> BasketItems { get; set; } = new();

    public static explicit operator BasketReturnDTO(Basket entity)
    {
        if (entity == null) return null;
        
        var dto = new  BasketReturnDTO
        {
            Id = entity.Id,
            BuyerId = entity.BuyerId,
           
        };
        dto.BasketItems.AddRange(entity.BasketItems.Select(x => (BasketItemReturnDto)x).ToList());
        
        return dto;
    }
}
