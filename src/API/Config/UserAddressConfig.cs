using API.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Config;

public class UserAddressConfig : IEntityTypeConfiguration<UserAddress>
{
    public void Configure(EntityTypeBuilder<UserAddress> b)
    {
        b.HasKey(x => x.Id);

        b.HasOne(x => x.User)
            .WithOne(x => x.Address)
            .HasForeignKey<UserAddress>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}