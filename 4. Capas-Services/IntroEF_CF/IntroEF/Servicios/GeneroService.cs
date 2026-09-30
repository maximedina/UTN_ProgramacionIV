
using AutoMapper;
using IntroEF.DTOs;
using IntroEF.Entidades;
using IntroEF.Excepciones;
using Microsoft.EntityFrameworkCore;

namespace IntroEF.Servicios
{
    public class GeneroService : IGeneroService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GeneroService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<Genero>> ObtenerTodos() =>
            await context.Generos.ToListAsync();

        public async Task Crear(GeneroCreacionDTO generoCreacion)
        {
            var yaExisteGeneroConEsteNombre = await context.Generos.AnyAsync(g =>
                g.Nombre == generoCreacion.Nombre);

            if (yaExisteGeneroConEsteNombre)
            {
                throw new ReglaDeNegocioException(
                    "Ya existe un género con el nombre " + generoCreacion.Nombre);
            }

            var genero = mapper.Map<Genero>(generoCreacion);
            context.Add(genero);
            await context.SaveChangesAsync();
        }

        public async Task CrearVarios(GeneroCreacionDTO[] generosCreacionDTO)
        {
            var generos = mapper.Map<Genero[]>(generosCreacionDTO);
            context.AddRange(generos);
            await context.SaveChangesAsync();
        }

        public async Task<bool> ActualizarNombreConSufijo(int id)
        {
            var genero = await context.Generos.FirstOrDefaultAsync(g => g.Id == id);

            if (genero is null)
            {
                return false;
            }

            genero.Nombre = genero.Nombre + "2";
            await context.SaveChangesAsync();
            return true;
        }

        public async Task Actualizar(int id, GeneroCreacionDTO generoCreacionDTO)
        {
            var genero = mapper.Map<Genero>(generoCreacionDTO);
            genero.Id = id;
            context.Update(genero);
            await context.SaveChangesAsync();
        }

        public async Task<int> BorrarModerno(int id) =>
            await context.Generos.Where(g => g.Id == id).ExecuteDeleteAsync();

        public async Task<bool> BorrarAnterior(int id)
        {
            var genero = await context.Generos.FirstOrDefaultAsync(g => g.Id == id);

            if (genero is null)
            {
                return false;
            }

            context.Remove(genero);
            await context.SaveChangesAsync();
            return true;
        }
    }
}