namespace API.DTOs;

public class BasketReturnDTO
{
    public int Id { get; set; }
    public string BuyerId { get; set; }
    public List<BasketItemReturnDto> BasketItems { get; set; } = new();
}
