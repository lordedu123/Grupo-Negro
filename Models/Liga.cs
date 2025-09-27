using System.ComponentModel.DataAnnotations;

namespace Grupo_negro.Models
{
    public class Liga
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Pais { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        public string? LogoUrl { get; set; }

        // Relaciones
        public ICollection<Equipo> Equipos { get; set; } = new List<Equipo>();
        public ICollection<Partido> Partidos { get; set; } = new List<Partido>();
    }
}