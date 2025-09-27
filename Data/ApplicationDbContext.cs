using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Grupo_negro.Models;

namespace Grupo_negro.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        
        // DbSets para el sistema de apuestas deportivas
        public DbSet<Liga> Ligas { get; set; }
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<Partido> Partidos { get; set; }
        public DbSet<Apuesta> Apuestas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuraciones adicionales si son necesarias
            builder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombres).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Apellidos).IsRequired().HasMaxLength(100);
                entity.Property(e => e.DNI).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Celular).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Correo).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Negocio).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(200);
            });

            // Configuraciones para el sistema de apuestas
            builder.Entity<Partido>(entity =>
            {
                entity.HasOne(p => p.EquipoLocal)
                    .WithMany(e => e.PartidosLocal)
                    .HasForeignKey(p => p.EquipoLocalId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.EquipoVisitante)
                    .WithMany(e => e.PartidosVisitante)
                    .HasForeignKey(p => p.EquipoVisitanteId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Liga)
                    .WithMany(l => l.Partidos)
                    .HasForeignKey(p => p.LigaId);
            });

            builder.Entity<Equipo>(entity =>
            {
                entity.HasOne(e => e.Liga)
                    .WithMany(l => l.Equipos)
                    .HasForeignKey(e => e.LigaId);
            });

            builder.Entity<Apuesta>(entity =>
            {
                entity.HasOne(a => a.Partido)
                    .WithMany(p => p.Apuestas)
                    .HasForeignKey(a => a.PartidoId);
            });
        }
    }
}