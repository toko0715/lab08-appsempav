using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;
namespace Biblioteca.Web.Controllers;
public class SociosController(SocioRepositorio repositorio) : Controller
{
    public async Task<IActionResult> Index() => View(await repositorio.ListarAsync());
    public IActionResult Create() => View(new Socio());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("DNI,Nombre,Email")] Socio socio)
    {
        if (!ModelState.IsValid) return View(socio);
        try
        {
            await repositorio.CrearAsync(socio);
        }
        catch (DniDuplicadoException)
        {
            ModelState.AddModelError(nameof(Socio.DNI), "Ya existe un socio con este DNI, incluso si está inactivo.");
            return View(socio);
        }
        TempData["Mensaje"] = "Socio registrado correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
