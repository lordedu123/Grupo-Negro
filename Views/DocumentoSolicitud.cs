namespace Grupo_negro.Models
{
    public class DocumentoSolicitud
    {
    public int Id { get; set; }
    public string NombreDocumento { get; set; } = string.Empty;
    public string RutaArchivo { get; set; } = string.Empty;

    // Relación con la solicitud
    public SolicitudLocal SolicitudLocal { get; set; } = null!;
    public int SolicitudLocalId { get; set; }
    }
}
