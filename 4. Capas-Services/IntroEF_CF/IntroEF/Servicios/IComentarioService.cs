using IntroEF.DTOs;

namespace IntroEF.Servicios
{
    public interface IComentarioService
    {
        Task Crear(int peliculaId, ComentarioCreacionDTO comentarioCreacionDTO);
    }
}