using System.ComponentModel.DataAnnotations;
namespace Biblioteca.Web.Models;
public class Libro
{
    public int LibroId { get; set; }
    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(200)]
    [Display(Name = "Título")]
    public string Titulo { get; set; } = "";
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un autor.")]
    [Display(Name = "Autor")]
    public int AutorId { get; set; }
    [Required(ErrorMessage = "El ISBN es obligatorio.")]
    [StringLength(20)]
    [DataType(DataType.Text)]
    public string ISBN { get; set; } = "";
    [Range(0, 100000, ErrorMessage = "Los ejemplares deben estar entre 0 y 100000.")]
    public int Ejemplares { get; set; }
    public bool Activo { get; set; } = true;
    public string? AutorNombre { get; set; }
}
