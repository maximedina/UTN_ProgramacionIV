using IntroEF.DTOs;
using IntroEF.Entidades;
using IntroEF.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace IntroEF.Controllers
{
    [ApiController]
    [Route("api/generos")]
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

        [HttpPost]
        public async Task<ActionResult> Post(GeneroCreacionDTO generoCreacion)
        {
            await servicio.Crear(generoCreacion);
            return Ok();
        }

        [HttpPost("varios")]
        public async Task<ActionResult> Post(GeneroCreacionDTO[] generosCreacionDTO)
        {
            await servicio.CrearVarios(generosCreacionDTO);
            return Ok();
        }

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

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, GeneroCreacionDTO generoCreacionDTO)
        {
            await servicio.Actualizar(id, generoCreacionDTO);
            return Ok();
        }

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