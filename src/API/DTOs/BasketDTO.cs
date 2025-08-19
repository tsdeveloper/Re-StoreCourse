using API.Entities.Baskets;

namespace API.DTOs;

public class BasketReturnDTO
{
    public int Id { get; set; }
    public string BuyerId { get; set; }
    public List<BasketItemReturnDto> BasketItems { get; set; } = new();

    public static explicit operator BasketReturnDTO(Basket entity)
    {
        var dto = new  BasketReturnDTO
        {
            Id = entity.Id,
            BuyerId = entity.BuyerId,
           
        };
        dto.BasketItems.AddRange(entity.BasketItems.Select(x => (BasketItemReturnDto)x).ToList());
        
        return dto;
    }
}
