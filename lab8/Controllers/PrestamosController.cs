using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;
namespace Biblioteca.Web.Controllers;
public class PrestamosController(PrestamoRepositorio repositorio) : Controller
{
    public async Task<IActionResult> Reporte(DateTime? desde, DateTime? hasta)
    {
        var inicio = desde?.Date ?? DateTime.Today.AddDays(-30);
        var fin = hasta?.Date ?? DateTime.Today;
        ViewData["Desde"] = desde?.ToString("yyyy-MM-dd") ?? inicio.ToString("yyyy-MM-dd");
        ViewData["Hasta"] = hasta?.ToString("yyyy-MM-dd") ?? fin.ToString("yyyy-MM-dd");
        if (inicio > fin) ModelState.AddModelError("", "La fecha inicial no puede ser posterior a la final.");
        if (!ModelState.IsValid) return View(Array.Empty<PrestamoReporte>());
        return View(await repositorio.ReporteAsync(inicio, fin));
    }
}
