namespace API.DTOs;

public class BaskReturnDTO
{
    public int Id { get; set; }
    public string BuyerId { get; set; }
    public ICollection<BasketItemDto> BasketItems { get; set; }
}
