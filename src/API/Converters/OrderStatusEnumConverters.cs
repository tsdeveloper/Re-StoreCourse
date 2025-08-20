using System.Linq.Expressions;
using API.Entities.Enum;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Converters;

public class OrderStatusEnumConverters : ValueConverter<OrderStatus, string>
{
    public OrderStatusEnumConverters()
    : base(
        v => v.ToString(),
        v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v)
        ) { }
}