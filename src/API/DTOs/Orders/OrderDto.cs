using API.DTOs.ShippingAddresses;
using API.Entities.Aggregate;

namespace API.DTOs.Orders;

public class OrderDto
{
    public int Id { get; set; }
    public string BuyerId { get; set; }
    public ShippingAddressDto ShippingAddress { get; set; }
    public string OrderDate { get; set; }
    public List<OrderItemDto> OrderItems { get; set; } = new();
    public decimal SubTotal { get; set; }
    public decimal DeliveryFee { get; set; }
    public string OrderStatus { get; set; }

    public decimal GetTotal()
    {
        return SubTotal + DeliveryFee;
    }

    public static explicit operator OrderDto(Order entity)
    {
        return new OrderDto
        {
            Id = entity.Id,
            BuyerId = entity.BuyerId,
            ShippingAddress = (ShippingAddressDto)entity.ShippingAddress,
            OrderDate = entity.OrderDate.ToString("yyyy-MM-dd hh:mm:ss"),
            OrderItems = entity.OrderItems.Select(x => (OrderItemDto)x).ToList(),
            SubTotal = entity.SubTotal,
            DeliveryFee = entity.DeliveryFee,
            OrderStatus = entity.OrderStatus.ToString()
        };
    }
}

public class OrderItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string PictureUrl { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public static explicit operator OrderItemDto(OrderItem entity)
    {
        return new OrderItemDto
        {
            Id = entity.Id,
            ProductId = entity.ItemOrdered.ProductId,
            Name = entity.ItemOrdered.Name,
            PictureUrl = entity.ItemOrdered.PictureUrl,
            Price = entity.Price,
            Quantity = entity.Quantity
        };
    }
}

public class CreateOrderDto
{
    public bool SaveAddress { get; set; }
    public ShippingAddressDto ShippingAddress { get; set; }
}