using System;
using System.Collections.Generic;

namespace IntroEF.Entidades;

public partial class Actore
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Fortuna { get; set; }

    public DateTime FechaNacimiento { get; set; }

    public virtual ICollection<PeliculasActore> PeliculasActores { get; set; } = new List<PeliculasActore>();
}
