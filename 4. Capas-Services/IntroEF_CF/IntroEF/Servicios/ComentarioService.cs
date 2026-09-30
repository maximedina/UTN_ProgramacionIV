using AutoMapper;
using IntroEF.DTOs;
using IntroEF.Entidades;

namespace IntroEF.Servicios
{
    public class ComentarioService : IComentarioService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ComentarioService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task Crear(int peliculaId, ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.PeliculaId = peliculaId;
            context.Add(comentario);
            await context.SaveChangesAsync();
        }
    }
}