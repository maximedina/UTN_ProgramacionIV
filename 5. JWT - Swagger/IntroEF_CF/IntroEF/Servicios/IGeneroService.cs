
using IntroEF.DTOs;
using IntroEF.Entidades;

namespace IntroEF.Servicios
{
    public interface IGeneroService
    {
        Task<List<Genero>> ObtenerTodos();
        Task Crear(GeneroCreacionDTO generoCreacion);
        Task CrearVarios(GeneroCreacionDTO[] generosCreacionDTO);
        Task<bool> ActualizarNombreConSufijo(int id);
        Task Actualizar(int id, GeneroCreacionDTO generoCreacionDTO);
        Task<int> BorrarModerno(int id);
        Task<bool> BorrarAnterior(int id);
    }
}