using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Biblioteca.Web.Controllers;
public class LibrosController(LibroRepositorio repositorio) : Controller
{
    public async Task<IActionResult> Index(string? titulo)
    {
        ViewData["Busqueda"] = titulo;
        return View(await repositorio.ListarAsync(titulo));
    }
    public async Task<IActionResult> Details(int id)
    {
        var libro = await repositorio.ObtenerAsync(id);
        return libro is null ? NotFound() : View(libro);
    }
    public async Task<IActionResult> Create()
    {
        await CargarAutoresAsync();
        return View(new Libro());
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Titulo,AutorId,ISBN,Ejemplares")] Libro libro)
    {
        await ValidarAutorAsync(libro);
        if (!ModelState.IsValid) return View(libro);
        try
        {
            await repositorio.CrearAsync(libro);
        }
        catch (IsbnDuplicadoException)
        {
            ModelState.AddModelError(nameof(Libro.ISBN), "Ya existe un libro con este ISBN, incluso si está inactivo.");
            return View(libro);
        }
        TempData["Mensaje"] = "Libro registrado correctamente.";
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(int id)
    {
        var libro = await repositorio.ObtenerAsync(id);
        if (libro is null) return NotFound();
        await CargarAutoresAsync(libro.AutorId);
        return View(libro);
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("LibroId,Titulo,AutorId,ISBN,Ejemplares")] Libro libro)
    {
        if (id != libro.LibroId) return BadRequest();
        if (await repositorio.ObtenerAsync(id) is null) return NotFound();
        await ValidarAutorAsync(libro);
        if (!ModelState.IsValid) return View(libro);
        try
        {
            await repositorio.ActualizarAsync(libro);
        }
        catch (IsbnDuplicadoException)
        {
            ModelState.AddModelError(nameof(Libro.ISBN), "Ya existe un libro con este ISBN, incluso si está inactivo.");
            return View(libro);
        }
        TempData["Mensaje"] = "Libro actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int id)
    {
        var libro = await repositorio.ObtenerAsync(id);
        return libro is null ? NotFound() : View(libro);
    }
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var libro = await repositorio.ObtenerAsync(id);
        if (libro is null) return NotFound();
        if (!ModelState.IsValid) return View("Delete", libro);
        await repositorio.EliminarAsync(id);
        TempData["Mensaje"] = "Libro desactivado correctamente.";
        return RedirectToAction(nameof(Index));
    }
    private async Task CargarAutoresAsync(int? seleccionado = null)
    {
        ViewData["Autores"] = new SelectList(await repositorio.ListarAutoresAsync(), "AutorId", "Nombre", seleccionado);
    }
    private async Task ValidarAutorAsync(Libro libro)
    {
        var autores = (await repositorio.ListarAutoresAsync()).ToList();
        ViewData["Autores"] = new SelectList(autores, "AutorId", "Nombre", libro.AutorId);
        if (!autores.Any(a => a.AutorId == libro.AutorId)) ModelState.AddModelError(nameof(Libro.AutorId), "Selecciona un autor activo.");
    }
}
