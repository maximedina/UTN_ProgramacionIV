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

            var usuario = new Usuario
            {
                Email = credenciales.Email,
                Password = credenciales.Password
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

            return new RespuestaAutenticacionDTO { Token = token, Expiracion = expiracion };
        }
    }
}