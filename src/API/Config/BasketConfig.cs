using API.Entities.Baskets;
using API.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Config;

public class BasketConfig : IEntityTypeConfiguration<Basket>
{
  public void Configure(EntityTypeBuilder<Basket> b)
  {
    b.HasKey(x => x.Id);

    b.Property(x => x.BuyerId).HasMaxLength(150).IsRequired();
    b.Property(x => x.CreatedAt).HasDefaultValueSql("getdate()").IsRequired();
    b.Property(x => x.UpdateAt).IsRequired(false);
        
    b.HasMany(x => x.BasketItems)
      .WithOne(x => x.Basket)
      .HasForeignKey(x => x.BasketId);
  }
}
