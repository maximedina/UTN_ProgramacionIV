using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace IntroEF.Entidades;

public partial class IntroEfdfContext : DbContext
{
    public IntroEfdfContext()
    {
    }

    public IntroEfdfContext(DbContextOptions<IntroEfdfContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Actore> Actores { get; set; }

    public virtual DbSet<Comentario> Comentarios { get; set; }

    public virtual DbSet<Genero> Generos { get; set; }

    public virtual DbSet<Pelicula> Peliculas { get; set; }

    public virtual DbSet<PeliculasActore> PeliculasActores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=IntroEFDF;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Actore>(entity =>
        {
            entity.Property(e => e.Fortuna).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Nombre).HasMaxLength(150);
        });

        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasIndex(e => e.PeliculaId, "IX_Comentarios_PeliculaId");

            entity.Property(e => e.Contenido).HasMaxLength(500);

            entity.HasOne(d => d.Pelicula).WithMany(p => p.Comentarios).HasForeignKey(d => d.PeliculaId);
        });

        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasIndex(e => e.Nombre, "IX_Generos_Nombre").IsUnique();

            entity.Property(e => e.Nombre).HasMaxLength(150);

            entity.HasMany(d => d.Peliculas).WithMany(p => p.Generos)
                .UsingEntity<Dictionary<string, object>>(
                    "GeneroPelicula",
                    r => r.HasOne<Pelicula>().WithMany().HasForeignKey("PeliculasId"),
                    l => l.HasOne<Genero>().WithMany().HasForeignKey("GenerosId"),
                    j =>
                    {
                        j.HasKey("GenerosId", "PeliculasId");
                        j.ToTable("GeneroPelicula");
                        j.HasIndex(new[] { "PeliculasId" }, "IX_GeneroPelicula_PeliculasId");
                    });
        });

        modelBuilder.Entity<Pelicula>(entity =>
        {
            entity.Property(e => e.Titulo).HasMaxLength(200);
        });

        modelBuilder.Entity<PeliculasActore>(entity =>
        {
            entity.HasKey(e => new { e.ActorId, e.PeliculaId });

            entity.HasIndex(e => e.PeliculaId, "IX_PeliculasActores_PeliculaId");

            entity.Property(e => e.Personaje).HasMaxLength(150);

            entity.HasOne(d => d.Actor).WithMany(p => p.PeliculasActores).HasForeignKey(d => d.ActorId);

            entity.HasOne(d => d.Pelicula).WithMany(p => p.PeliculasActores).HasForeignKey(d => d.PeliculaId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
