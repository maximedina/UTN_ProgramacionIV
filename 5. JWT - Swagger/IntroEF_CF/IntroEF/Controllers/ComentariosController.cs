using IntroEF.DTOs;
using IntroEF.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Post(int peliculaId,
            ComentarioCreacionDTO comentarioCreacionDTO)
        {
            await servicio.Crear(peliculaId, comentarioCreacionDTO);
            return Ok();
        }
    }
}