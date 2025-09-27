namespace Grupo_negro.Models
{
    public class SolicitudLocal
    {
    public int Id { get; set; }
    public string Solicitante { get; set; } = string.Empty;
    public string DocumentoIdentidad { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Distrito { get; set; } = string.Empty;
    public string DireccionLocal { get; set; } = string.Empty;
    public string Estado { get; set; } = "Pendiente"; // valores: Pendiente, En Revisión, Aprobado, Rechazado

        // Respuesta del admin
        public string? RespuestaAdmin { get; set; } = string.Empty;

        // Comentarios del usuario tras respuesta
        public string ComentariosAdicionales  { get; set; } = string.Empty;

    // Documentos asociados
        public ICollection<DocumentoSolicitud> Documentos { get; set; } = new List<DocumentoSolicitud>();
    }
}
