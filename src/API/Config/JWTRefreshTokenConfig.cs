using API.Entities.JWT;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Config;

public class JWTRefreshTokenConfig : IEntityTypeConfiguration<JWTRefreshToken>
{
    public void Configure(EntityTypeBuilder<JWTRefreshToken> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.Token).HasMaxLength(200).IsRequired();
        b.Property(x => x.ExpiredAt).IsRequired();
        b.Property(x => x.IsRevoked).IsRequired();
        b.Property(x => x.CreatedAt)
            .HasDefaultValueSql("getdate()").IsRequired();
        b.Property(x => x.UpdateAt).IsRequired(false);
        b.HasOne(x => x.User)
            .WithMany(x => x.RefreshTokenList)
            .HasForeignKey(x => x.UserId);
    }
}