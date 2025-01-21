namespace API.DTOs;

public class BasketItemDto
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public int ProductId { get; set; }
    public int BasketId { get; set; }
}