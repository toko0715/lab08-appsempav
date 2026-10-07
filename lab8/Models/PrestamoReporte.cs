using System.ComponentModel.DataAnnotations;
namespace Biblioteca.Web.Models;
public class PrestamoReporte
{
    public int PrestamoId { get; set; }
    public string Socio { get; set; } = "";
    public string Libro { get; set; } = "";
    [DataType(DataType.Date)]
    public DateTime FechaPrestamo { get; set; }
    [DataType(DataType.Date)]
    public DateTime FechaLimite { get; set; }
    public string Estado { get; set; } = "";
}
