using API.Entities.Baskets;
using API.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Config;

public class BasketItemConfig : IEntityTypeConfiguration<BasketItem>
{
  public void Configure(EntityTypeBuilder<BasketItem> b)
  {
    b.HasKey(x => x.Id);

    b.Property(x => x.Quantity).IsRequired();
    b.Property(x => x.CreatedAt).HasDefaultValueSql("getdate()").IsRequired();
    b.Property(x => x.UpdateAt).IsRequired(false);
  }
}
