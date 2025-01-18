using API.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Config;

public class ProductConfig : IEntityTypeConfiguration<Product>
{
  public void Configure(EntityTypeBuilder<Product> b)
  {
    b.HasKey(x => x.Id);

    b.Property(x => x.Name).HasMaxLength(150).IsRequired();
    b.Property(x => x.Description).HasMaxLength(200).IsRequired(false);
    b.Property(x => x.Price).IsRequired(true);
    b.Property(x => x.PictureUrl).HasMaxLength(250).IsRequired(false);
    b.HasOne(x => x.Type).WithMany(o => o.ProductList).HasForeignKey(x => x.TypeId);
    b.HasOne(x => x.Brand).WithMany(o => o.ProductList).HasForeignKey(x => x.BrandId);
    b.Property(x => x.CreatedAt).HasDefaultValueSql("getdate()").IsRequired();
    b.Property(x => x.UpdateAt).IsRequired(false);

  }
}
