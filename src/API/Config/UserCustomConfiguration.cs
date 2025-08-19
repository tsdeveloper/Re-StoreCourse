using API.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Config;

public class UserCustomConfiguration : IEntityTypeConfiguration<UserCustom>
{
    public void Configure(EntityTypeBuilder<UserCustom> builder)
    {
        // Define o nome da tabela para o usuário
        builder.ToTable("Users");

        // Configura as propriedades do IdentityUser
        builder.Property(u => u.Id)
            .HasMaxLength(256);

        builder.Property(u => u.Email)
            .HasMaxLength(256);

        builder.Property(u => u.NormalizedEmail)
            .HasMaxLength(256);

        builder.Property(u => u.UserName)
            .HasMaxLength(256);

        builder.Property(u => u.NormalizedUserName)
            .HasMaxLength(256);

        // // Se você tiver propriedades adicionais na sua classe UserCustom,
        // // configure-as aqui
        // builder.Property(u => u.NomeCompleto)
        //     .HasMaxLength(150);
        //
        // // Exemplo de índice único para uma propriedade personalizada
        // builder.HasIndex(u => u.Documento)
        //     .IsUnique();

        // Configura a chave primária
        builder.HasKey(u => u.Id);

        // Configura as relações de 1 para muitos, se existirem.
        // Exemplo: builder.HasMany(u => u.Pedidos).WithOne(p => p.Usuario).HasForeignKey(p => p.UsuarioId);

        // Configura o relacionamento do Identity com Roles
        builder.HasMany<IdentityUserRole<string>>()
            .WithOne()
            .HasForeignKey(ur =>  ur.UserId)
            .IsRequired();

        // Configura o relacionamento do Identity com Logins
        builder.HasMany<IdentityUserLogin<string>>()
            .WithOne()
            .HasForeignKey(ul => ul.UserId)
            .IsRequired();

        // Configura o relacionamento do Identity com Claims
        builder.HasMany<IdentityUserClaim<string>>()
            .WithOne()
            .HasForeignKey(uc => uc.UserId)
            .IsRequired();
    }
}