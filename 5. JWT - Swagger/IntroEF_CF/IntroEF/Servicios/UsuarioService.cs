using IntroEF.DTOs;
using IntroEF.Entidades;
using IntroEF.Excepciones;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IntroEF.Servicios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;

        public UsuarioService(ApplicationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public async Task Registrar(CredencialesUsuarioDTO credenciales)
        {
            var yaExisteUsuarioConEsteEmail = await context.Usuarios
                .AnyAsync(u => u.Email == credenciales.Email);

            if (yaExisteUsuarioConEsteEmail)
            {
                throw new ReglaDeNegocioException(
                    "Ya existe un usuario con el email " + credenciales.Email);
            }

            // Nota para la clase: contraseña en texto plano solo por simplicidad.
            // En un proyecto real se usa PasswordHasher<T> o ASP.NET Core Identity.
            var usuario = new Usuario
            {
                Email = credenciales.Email,
                Password = credenciales.Password,
                Rol = Roles.User // el registro público SIEMPRE crea un User
            };

            context.Add(usuario);
            await context.SaveChangesAsync();
        }

        public async Task<Usuario?> ValidarCredenciales(CredencialesUsuarioDTO credenciales) =>
            await context.Usuarios.FirstOrDefaultAsync(u =>
                u.Email == credenciales.Email && u.Password == credenciales.Password);

        public RespuestaAutenticacionDTO ConstruirToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(ClaimTypes.Role, usuario.Rol),
            };

            var llave = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["JwtKey"]!));
            var credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddHours(1);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: expiracion,
                signingCredentials: credenciales);

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return new RespuestaAutenticacionDTO
            {
                Token = token,
                Expiracion = expiracion,
                Rol = usuario.Rol
            };
        }

        public async Task<bool> CambiarRol(int id, string rol)
        {
            if (!Roles.Todos.Contains(rol))
            {
                throw new ReglaDeNegocioException(
                    $"El rol '{rol}' no es válido. Roles permitidos: {string.Join(", ", Roles.Todos)}");
            }

            var usuario = await context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

            if (usuario is null)
            {
                return false;
            }

            usuario.Rol = rol;
            await context.SaveChangesAsync();
            return true;
        }
    }
}