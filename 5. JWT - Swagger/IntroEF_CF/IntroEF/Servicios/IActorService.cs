using IntroEF.DTOs;
using IntroEF.Entidades;

namespace IntroEF.Servicios
{
    public interface IActorService
    {
        Task<List<Actor>> ObtenerTodosPorFechaNacimiento();
        Task<List<Actor>> ObtenerPorNombreExacto(string nombre);
        Task<List<Actor>> ObtenerPorNombreParcial(string nombre);
        Task<Actor?> ObtenerPorId(int id);
        Task<List<ActorDTO>> ObtenerIdYNombre();
        Task Crear(ActorCreacionDTO actorCreacionDTO);
    }
}