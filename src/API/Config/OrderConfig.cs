using API.Converters;
using API.Entities.Aggregate;
using API.Entities.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Config;

public class OrderConfig : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.BuyerId)
            .HasMaxLength(150)
            .IsRequired();
        
        b.Property(x => x.OrderDate)
            .IsRequired();
        
        b.Property(x => x.SubTotal)
            .IsRequired();
        
        b.Property(x => x.DeliveryFee)
            .IsRequired();
        
        b.Property(x => x.PaymentIntendId).IsRequired(false);

        
        b.Property(x => x.OrderStatus)
            .HasDefaultValue(OrderStatus.Pending)
            .HasConversion<OrderStatusEnumConverters>();
    }
}