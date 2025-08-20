using API.Entities.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Config;

public class RoleCustomConfig : IEntityTypeConfiguration<RoleCustom>
{
    public void Configure(EntityTypeBuilder<RoleCustom> b)
    {
        b.HasKey(x => x.Id);
        
        b.HasMany<IdentityUserRole<int>>()
            .WithOne()
            .HasForeignKey(ur =>  ur.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}