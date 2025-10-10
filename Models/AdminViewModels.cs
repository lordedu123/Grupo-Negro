using Grupo_negro.Models;

namespace Grupo_negro.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalUsuarios { get; set; }
        public int TotalApuestas { get; set; }
        public int ApuestasActivas { get; set; }
        public int ApuestasGanadas { get; set; }
        public int ApuestasPerdidas { get; set; }
        public decimal TotalMontoApostado { get; set; }
        public List<Apuesta> ApuestasRecientes { get; set; } = new();
    }
}