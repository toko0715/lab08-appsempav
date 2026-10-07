using System.Data;
using Biblioteca.Web.Models;
using Dapper;
using Microsoft.Data.SqlClient;
namespace Biblioteca.Web.Repositorios;
public class LibroRepositorio(IConfiguration configuration)
{
    private readonly string connectionString = configuration.GetConnectionString("BibliotecaDB")
        ?? throw new InvalidOperationException("Falta ConnectionStrings:BibliotecaDB.");
    public async Task<IEnumerable<Libro>> ListarAsync(string? titulo)
    {
        await using var connection = new SqlConnection(connectionString);
        return string.IsNullOrWhiteSpace(titulo)
            ? await connection.QueryAsync<Libro>("dbo.usp_Libros_Listar", commandType: CommandType.StoredProcedure)
            : await connection.QueryAsync<Libro>("dbo.usp_Libros_Buscar", new { Titulo = titulo.Trim() }, commandType: CommandType.StoredProcedure);
    }
    public async Task<Libro?> ObtenerAsync(int id)
    {
        await using var connection = new SqlConnection(connectionString);
        return await connection.QuerySingleOrDefaultAsync<Libro>("dbo.usp_Libros_Obtener", new { LibroId = id }, commandType: CommandType.StoredProcedure);
    }
    public async Task<IEnumerable<Autor>> ListarAutoresAsync()
    {
        await using var connection = new SqlConnection(connectionString);
        return await connection.QueryAsync<Autor>("dbo.usp_Autores_Listar", commandType: CommandType.StoredProcedure);
    }
    public async Task CrearAsync(Libro libro)
    {
        await using var connection = new SqlConnection(connectionString);
        try
        {
            await connection.ExecuteAsync("dbo.usp_Libros_Insertar", new { libro.Titulo, libro.AutorId, libro.ISBN, libro.Ejemplares }, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException ex) when (ex.Number == 50002)
        {
            throw new IsbnDuplicadoException();
        }
    }
    public async Task ActualizarAsync(Libro libro)
    {
        await using var connection = new SqlConnection(connectionString);
        try
        {
            await connection.ExecuteAsync("dbo.usp_Libros_Actualizar", new { libro.LibroId, libro.Titulo, libro.AutorId, libro.ISBN, libro.Ejemplares }, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException ex) when (ex.Number == 50002)
        {
            throw new IsbnDuplicadoException();
        }
    }
    public async Task EliminarAsync(int id)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.ExecuteAsync("dbo.usp_Libros_Eliminar", new { LibroId = id }, commandType: CommandType.StoredProcedure);
    }
}
