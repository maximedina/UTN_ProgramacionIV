using IntroEF.DTOs;
using IntroEF.Entidades;
using IntroEF.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntroEF.Controllers
{
    [ApiController]
    [Route("api/generos")]
    [Tags("Géneros")]
    public class GenerosController : ControllerBase
    {
        private readonly IGeneroService servicio;

        public GenerosController(IGeneroService servicio)
        {
            this.servicio = servicio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genero>>> Get() =>
            await servicio.ObtenerTodos();

        /// <summary>
        /// Crea un nuevo género.
        /// </summary>
        /// <param name="generoCreacion">Nombre del género a crear.</param>
        /// <response code="200">El género se creó correctamente.</response>
        /// <response code="400">Ya existe un género con ese nombre.</response>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Post(GeneroCreacionDTO generoCreacion)
        {
            await servicio.Crear(generoCreacion);
            return Ok();
        }

        [Authorize]
        [HttpPost("varios")]
        public async Task<ActionResult> Post(GeneroCreacionDTO[] generosCreacionDTO)
        {
            await servicio.CrearVarios(generosCreacionDTO);
            return Ok();
        }

        [Authorize]
        [HttpPut("{id:int}/nombre2")]
        public async Task<ActionResult> Put(int id)
        {
            var actualizado = await servicio.ActualizarNombreConSufijo(id);

            if (!actualizado)
            {
                return NotFound();
            }

            return Ok();
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, GeneroCreacionDTO generoCreacionDTO)
        {
            await servicio.Actualizar(id, generoCreacionDTO);
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

        [Authorize]
        [HttpDelete("{id:int}/anterior")]
        public async Task<ActionResult> DeleteAnterior(int id)
        {
            var borrado = await servicio.BorrarAnterior(id);

            if (!borrado)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}