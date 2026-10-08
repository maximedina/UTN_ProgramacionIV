using AutoMapper;
using AutoMapper.QueryableExtensions;
using IntroEF.DTOs;
using IntroEF.Entidades;
using Microsoft.EntityFrameworkCore;

namespace IntroEF.Servicios
{
    public class ActorService : IActorService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ActorService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<Actor>> ObtenerTodosPorFechaNacimiento() =>
            await context.Actores.OrderByDescending(a => a.FechaNacimiento).ToListAsync();

        public async Task<List<Actor>> ObtenerPorNombreExacto(string nombre) =>
            // Versión 1
            await context.Actores
                .Where(a => a.Nombre == nombre)
                .OrderBy(a => a.Nombre)
                    .ThenByDescending(a => a.FechaNacimiento)
                .ToListAsync();

        public async Task<List<Actor>> ObtenerPorNombreParcial(string nombre) =>
            // Versión 2: Contiene
            await context.Actores.Where(a => a.Nombre.Contains(nombre)).ToListAsync();

        public async Task<Actor?> ObtenerPorId(int id) =>
            await context.Actores.FirstOrDefaultAsync(a => a.Id == id);

        public async Task<List<ActorDTO>> ObtenerIdYNombre() =>
            await context.Actores
                .ProjectTo<ActorDTO>(mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task Crear(ActorCreacionDTO actorCreacionDTO)
        {
            var actor = mapper.Map<Actor>(actorCreacionDTO);
            context.Add(actor);
            await context.SaveChangesAsync();
        }
    }
}