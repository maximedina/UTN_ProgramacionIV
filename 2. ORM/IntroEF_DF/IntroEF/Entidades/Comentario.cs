using System;
using System.Collections.Generic;

namespace IntroEF.Entidades;

public partial class Comentario
{
    public int Id { get; set; }

    public string? Contenido { get; set; }

    public bool Recomendar { get; set; }

    public int PeliculaId { get; set; }

    public virtual Pelicula Pelicula { get; set; } = null!;
}
