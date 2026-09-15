using System;
using System.Collections.Generic;

namespace IntroEF.Entidades;

public partial class Pelicula
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public bool EnCines { get; set; }

    public DateTime FechaEstreno { get; set; }

    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    public virtual ICollection<PeliculasActore> PeliculasActores { get; set; } = new List<PeliculasActore>();

    public virtual ICollection<Genero> Generos { get; set; } = new List<Genero>();
}
