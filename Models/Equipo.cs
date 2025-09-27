using System.ComponentModel.DataAnnotations;

namespace Grupo_negro.Models
{
    public class Equipo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Ciudad { get; set; }

        public string? EscudoUrl { get; set; }

        // Relación con Liga
        public int LigaId { get; set; }
        public Liga Liga { get; set; } = null!;

        // Relaciones
        public ICollection<Partido> PartidosLocal { get; set; } = new List<Partido>();
        public ICollection<Partido> PartidosVisitante { get; set; } = new List<Partido>();
    }
}