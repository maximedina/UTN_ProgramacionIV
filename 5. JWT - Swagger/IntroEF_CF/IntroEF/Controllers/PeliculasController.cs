using IntroEF.DTOs;
using IntroEF.Entidades;
using IntroEF.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntroEF.Controllers
{
    [ApiController]
    [Route("api/peliculas")]
    public class PeliculasController : ControllerBase
    {
        private readonly IPeliculaService servicio;

        public PeliculasController(IPeliculaService servicio)
        {
            this.servicio = servicio;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Pelicula>> Get(int id)
        {
            var pelicula = await servicio.ObtenerPorIdConDetalles(id);

            if (pelicula is null)
            {
                return NotFound();
            }

            return pelicula;
        }

        [HttpGet("select/{id:int}")]
        public async Task<ActionResult> GetSelect(int id)
        {
            var pelicula = await servicio.ObtenerSelectConDetalles(id);

            if (pelicula is null)
            {
                return NotFound();
            }

            return Ok(pelicula);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Post(PeliculaCreacionDTO peliculaCreacionDTO)
        {
            await servicio.Crear(peliculaCreacionDTO);
            return Ok();
        }

        [Authorize]
        [HttpDelete("{id:int}/moderna")]
        public async Task<ActionResult> Delete(int id)
        {
            var filasAlteradas = await servicio.BorrarModerno(id);

            if (filasAlteradas == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}