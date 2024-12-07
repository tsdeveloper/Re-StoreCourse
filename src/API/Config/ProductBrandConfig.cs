using API.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Config;

public class ProductBrandConfig : IEntityTypeConfiguration<ProductBrand>
{
  public void Configure(EntityTypeBuilder<ProductBrand> b)
  {
    b.HasKey(x => x.Id);

    b.Property(x => x.Name).HasMaxLength(100).IsRequired();
    b.Property(x => x.CreatedAt).HasDefaultValueSql("getdate()").IsRequired();
    b.Property(x => x.UpdateAt).IsRequired(false);

  }
}
