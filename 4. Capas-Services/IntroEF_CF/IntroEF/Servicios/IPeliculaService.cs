using IntroEF.DTOs;
using IntroEF.Entidades;

namespace IntroEF.Servicios
{
    public interface IPeliculaService
    {
        Task<Pelicula?> ObtenerPorIdConDetalles(int id);
        Task<object?> ObtenerSelectConDetalles(int id);
        Task Crear(PeliculaCreacionDTO peliculaCreacionDTO);
        Task<int> BorrarModerno(int id);
    }
}