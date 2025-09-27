using System.ComponentModel.DataAnnotations;

namespace Grupo_negro.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        
        [Required]
        public string Nombres { get; set; } = string.Empty;
        
        [Required]
        public string Apellidos { get; set; } = string.Empty;
        
        [Required]
        public string DNI { get; set; } = string.Empty;
        
        [Required]
        public string Celular { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Correo { get; set; } = string.Empty;
        
        [Required]
        public string Negocio { get; set; } = string.Empty;
        
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
