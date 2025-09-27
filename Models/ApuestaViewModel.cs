using System.ComponentModel.DataAnnotations;
using Grupo_negro.Models;

namespace Grupo_negro.Models
{
    // ViewModel para el formulario de apuesta
    public class ApuestaViewModel
    {
        public int PartidoId { get; set; }
        public string NombreEquipoLocal { get; set; } = string.Empty;
        public string NombreEquipoVisitante { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public string Liga { get; set; } = string.Empty;
        public decimal CuotaLocal { get; set; }
        public decimal CuotaEmpate { get; set; }
        public decimal CuotaVisitante { get; set; }

        [Required(ErrorMessage = "Selecciona el tipo de apuesta")]
        public TipoApuesta TipoApuesta { get; set; }

        [Required(ErrorMessage = "Ingresa el monto a apostar")]
        [Range(1, 10000, ErrorMessage = "El monto debe estar entre $1 y $10,000")]
        public decimal MontoApostado { get; set; }

        public decimal PosibleGanancia => TipoApuesta switch
        {
            TipoApuesta.GanaLocal => MontoApostado * CuotaLocal,
            TipoApuesta.Empate => MontoApostado * CuotaEmpate,
            TipoApuesta.GanaVisitante => MontoApostado * CuotaVisitante,
            _ => 0
        };
    }
}