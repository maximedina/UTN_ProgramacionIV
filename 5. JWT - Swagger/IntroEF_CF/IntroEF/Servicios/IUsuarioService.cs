using IntroEF.DTOs;
using IntroEF.Entidades;

namespace IntroEF.Servicios
{
    public interface IUsuarioService
    {
        Task Registrar(CredencialesUsuarioDTO credenciales);
        Task<Usuario?> ValidarCredenciales(CredencialesUsuarioDTO credenciales);
        RespuestaAutenticacionDTO ConstruirToken(Usuario usuario);
    }
}