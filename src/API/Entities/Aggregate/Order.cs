using API.Entities.Enum;

namespace API.Entities.Aggregate;

public class Order
{
    public int Id { get; set; }
    public string BuyerId { get; set; }
    public ShippingAddress ShippingAddress { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public List<OrderItem> OrderItems { get; set; } = new();
    public long SubTotal { get; set; }
    public long DeliveryFee { get; set; }
    public OrderStatus OrderStatus { get; set; }
    
}