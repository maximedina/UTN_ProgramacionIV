using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntroEF.Entidades.Configuraciones
{
    public class UsuarioConfig : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.Rol)
                .HasMaxLength(20)
                .HasDefaultValue(Roles.User);

            // Admin inicial, para poder probar sin tocar la base a mano
            builder.HasData(new Usuario
            {
                Id = 100,
                Email = "admin@ejemplo.com",
                Password = "Admin123!",
                Rol = Roles.Admin
            });
        }
    }
}