using IntroEF.DTOs;
using IntroEF.Entidades;
using IntroEF.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace IntroEF.Controllers
{
    [ApiController]
    [Route("api/actores")]
    public class ActoresController : ControllerBase
    {
        private readonly IActorService servicio;

        public ActoresController(IActorService servicio)
        {
            this.servicio = servicio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actor>>> Get() =>
            await servicio.ObtenerTodosPorFechaNacimiento();

        [HttpGet("nombre")]
        public async Task<ActionResult<IEnumerable<Actor>>> Get(string nombre) =>
            await servicio.ObtenerPorNombreExacto(nombre);

        [HttpGet("nombre/v2")]
        public async Task<ActionResult<IEnumerable<Actor>>> GetV2(string nombre) =>
            await servicio.ObtenerPorNombreParcial(nombre);

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Actor>> Get(int id)
        {
            var actor = await servicio.ObtenerPorId(id);

            if (actor is null)
            {
                return NotFound();
            }

            return actor;
        }

        [HttpGet("idynombre")]
        public async Task<ActionResult<IEnumerable<ActorDTO>>> Getidynombre() =>
            await servicio.ObtenerIdYNombre();

        [HttpPost]
        public async Task<ActionResult> Post(ActorCreacionDTO actorCreacionDTO)
        {
            await servicio.Crear(actorCreacionDTO);
            return Ok();
        }
    }
}