using IntroEF.DTOs;
using IntroEF.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IntroEF.Entidades;

namespace IntroEF.Controllers
{
    [ApiController]
    [Route("api/peliculas/{peliculaId:int}/comentarios")]
    public class ComentariosController : ControllerBase
    {
        private readonly IComentarioService servicio;

        public ComentariosController(IComentarioService servicio)
        {
            this.servicio = servicio;
        }
        //Cualquier usuario logueado
        //[Authorize]
        //COn cualquiera de los dos
        //[Authorize(Roles = "Admin,User")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.User}")]
        
        //Para requerir ambos
        //[Authorize(Roles = Roles.Admin)]
        //[Authorize(Roles = Roles.User)]
        [HttpPost]
        public async Task<ActionResult> Post(int peliculaId,
            ComentarioCreacionDTO comentarioCreacionDTO)
        {
            await servicio.Crear(peliculaId, comentarioCreacionDTO);
            return Ok();
        }
    }
}