using AutoMapper;
using IntroEF.DTOs;
using IntroEF.Entidades;
using Microsoft.EntityFrameworkCore;

namespace IntroEF.Servicios
{
    public class PeliculaService : IPeliculaService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public PeliculaService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Pelicula?> ObtenerPorIdConDetalles(int id) =>
            await context.Peliculas
                .Include(p => p.Comentarios)
                .Include(p => p.Generos)
                .Include(p => p.PeliculasActores.OrderBy(pa => pa.Orden))
                    .ThenInclude(pa => pa.Actor)
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<object?> ObtenerSelectConDetalles(int id) =>
            await context.Peliculas
                .Select(pel => new
                {
                    pel.Id,
                    pel.Titulo,
                    Generos = pel.Generos.Select(g => g.Nombre).ToList(),
                    Actores = pel.PeliculasActores.OrderBy(pa => pa.Orden).Select(pa =>
                    new
                    {
                        Id = pa.ActorId,
                        pa.Actor.Nombre,
                        pa.Personaje
                    }),
                    CantidadComentarios = pel.Comentarios.Count()
                })
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task Crear(PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var pelicula = mapper.Map<Pelicula>(peliculaCreacionDTO);

            if (pelicula.Generos is not null)
            {
                foreach (var genero in pelicula.Generos)
                {
                    context.Entry(genero).State = EntityState.Unchanged;
                }
            }

            if (pelicula.PeliculasActores is not null)
            {
                for (int i = 0; i < pelicula.PeliculasActores.Count; i++)
                {
                    pelicula.PeliculasActores[i].Orden = i + 1;
                }
            }

            context.Add(pelicula);
            await context.SaveChangesAsync();
        }

        public async Task<int> BorrarModerno(int id) =>
            await context.Peliculas.Where(g => g.Id == id).ExecuteDeleteAsync();
    }
}