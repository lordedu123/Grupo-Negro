using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Grupo_negro.Models
{
    public enum TipoApuesta
    {
        GanaLocal = 1,
        Empate = 2,
        GanaVisitante = 3
    }

    public enum EstadoApuesta
    {
        Activa,
        Ganada,
        Perdida,
        Cancelada
    }

    public class Apuesta
    {
        public int Id { get; set; }

        [Required]
        public string UsuarioId { get; set; } = string.Empty;
        public IdentityUser Usuario { get; set; } = null!;

        // Partido apostado
        public int PartidoId { get; set; }
        public Partido Partido { get; set; } = null!;

        // Detalles de la apuesta
        public TipoApuesta TipoApuesta { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Range(0.01, 10000)]
        public decimal MontoApostado { get; set; }

        [Column(TypeName = "decimal(4,2)")]
        public decimal CuotaAplicada { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PosibleGanancia { get; set; }

        public DateTime FechaApuesta { get; set; } = DateTime.Now;

        public EstadoApuesta Estado { get; set; } = EstadoApuesta.Activa;

        public DateTime? FechaResolucion { get; set; }
    }
}