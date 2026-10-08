using IntroEF.DTOs;
using IntroEF.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace IntroEF.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService servicio;

        public UsuariosController(IUsuarioService servicio)
        {
            this.servicio = servicio;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult> Registrar(CredencialesUsuarioDTO credenciales)
        {
            await servicio.Registrar(credenciales);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> Login(
            CredencialesUsuarioDTO credenciales)
        {
            var usuario = await servicio.ValidarCredenciales(credenciales);

            if (usuario is null)
            {
                return Unauthorized();
            }

            return servicio.ConstruirToken(usuario);
        }
    }
}
