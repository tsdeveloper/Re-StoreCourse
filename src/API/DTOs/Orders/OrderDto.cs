using API.DTOs.Products;
using API.DTOs.ShippingAddresses;
using API.Entities.Aggregate;
using API.Entities.Enum;

namespace API.DTOs.Orders;

public class OrderDto
{
    public int Id { get; set; }
    public string BuyerId { get; set; }
    public ShippingAddressDto ShippingAddress { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public List<OrderItemDto> OrderItems { get; set; } = new();
    public long SubTotal { get; set; }
    public long DeliveryFee { get; set; }
    public OrderStatus OrderStatus { get; set; }

    public long GetTotal()
    {
        return SubTotal + DeliveryFee;
    }

    public static explicit operator OrderDto(Order entity)
    {
        return new OrderDto
        {
            BuyerId = entity.BuyerId,
            ShippingAddress = (ShippingAddressDto)entity.ShippingAddress,
            OrderDate = entity.OrderDate,
            OrderItems = entity.OrderItems.Select(x => (OrderItemDto)x).ToList(),
            SubTotal = entity.SubTotal,
            DeliveryFee = entity.DeliveryFee,
            OrderStatus = entity.OrderStatus,
        };
    }

}

public class OrderItemDto
{
    public int Id { get; set; }
    public ProductItemOrderedDto ItemOrdered { get; set; }
    public long Price { get; set; }
    public int Quantity { get; set; }

    public static explicit operator OrderItemDto(OrderItem entity)
    {
        return new OrderItemDto
        {
            Id = entity.Id,
            ItemOrdered = (ProductItemOrderedDto)entity.ItemOrdered,
            Price = entity.Price,
            Quantity = entity.Quantity,
        };
    }
}