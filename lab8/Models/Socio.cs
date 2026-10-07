using System.ComponentModel.DataAnnotations;
namespace Biblioteca.Web.Models;
public class Socio
{
    public int SocioId { get; set; }
    [Required(ErrorMessage = "El DNI es obligatorio.")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe contener 8 dígitos.")]
    [StringLength(8)]
    public string DNI { get; set; } = "";
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(150)]
    public string Nombre { get; set; } = "";
    [EmailAddress(ErrorMessage = "Ingresa un correo válido.")]
    [StringLength(150)]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    public bool Activo { get; set; } = true;
}
