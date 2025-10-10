namespace Grupo_negro.Models
{
    public class LocalesFisicos
    {
    public int Id { get; set; }
    public string NombreLocal { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public string Estado { get; set; } = "Disponible";
    }
}
