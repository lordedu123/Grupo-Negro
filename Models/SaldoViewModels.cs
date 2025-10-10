using System.ComponentModel.DataAnnotations;

namespace Grupo_negro.Models
{
    public class DepositoViewModel
    {
        [Required(ErrorMessage = "El monto es requerido")]
        [Range(0.01, 10000.00, ErrorMessage = "El monto debe estar entre $0.01 y $10,000.00")]
        [Display(Name = "Monto a depositar")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "Selecciona un método de pago")]
        [Display(Name = "Método de pago")]
        public MetodoPago MetodoPago { get; set; }

        [Display(Name = "Número de teléfono (para Yape)")]
        public string? NumeroTelefono { get; set; }

        [Display(Name = "Email de PayPal")]
        public string? EmailPayPal { get; set; }
    }

    public class RetiroViewModel
    {
        [Required(ErrorMessage = "El monto es requerido")]
        [Range(0.01, 10000.00, ErrorMessage = "El monto debe estar entre $0.01 y $10,000.00")]
        [Display(Name = "Monto a retirar")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "Selecciona un método de retiro")]
        [Display(Name = "Método de retiro")]
        public MetodoPago MetodoRetiro { get; set; }

        [Display(Name = "Número de teléfono (para Yape)")]
        public string? NumeroTelefono { get; set; }

        [Display(Name = "Email de PayPal")]
        public string? EmailPayPal { get; set; }
    }

    public enum MetodoPago
    {
        [Display(Name = "PayPal")]
        PayPal = 1,
        [Display(Name = "Yape")]
        Yape = 2
    }
}