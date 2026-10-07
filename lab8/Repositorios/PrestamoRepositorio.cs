using System.Data;
using Biblioteca.Web.Models;
using Dapper;
using Microsoft.Data.SqlClient;
namespace Biblioteca.Web.Repositorios;
public class PrestamoRepositorio(IConfiguration configuration)
{
    private readonly string connectionString = configuration.GetConnectionString("BibliotecaDB")
        ?? throw new InvalidOperationException("Falta ConnectionStrings:BibliotecaDB.");
    public async Task<IEnumerable<PrestamoReporte>> ReporteAsync(DateTime desde, DateTime hasta)
    {
        await using var connection = new SqlConnection(connectionString);
        return await connection.QueryAsync<PrestamoReporte>("dbo.usp_Prestamos_Reporte", new { Desde = desde, Hasta = hasta }, commandType: CommandType.StoredProcedure);
    }
}
