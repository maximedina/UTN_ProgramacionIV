using IntroEF.DTOs;
using IntroEF.Servicios;
using Microsoft.AspNetCore.Mvc;
using IntroEF.Entidades;
using Microsoft.AspNetCore.Authorization;

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

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id:int}/rol")]
        public async Task<ActionResult> CambiarRol(int id, CambiarRolDTO cambiarRolDTO)
        {
            var actualizado = await servicio.CambiarRol(id, cambiarRolDTO.Rol);

            if (!actualizado)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
