using System.ComponentModel.DataAnnotations;

namespace IntroEF.Entidades
{
    public class Genero
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public HashSet<Pelicula> Peliculas { get; set; } = new HashSet<Pelicula>();
    }
}
