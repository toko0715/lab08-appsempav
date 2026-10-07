using System.Data;
using Biblioteca.Web.Models;
using Dapper;
using Microsoft.Data.SqlClient;
namespace Biblioteca.Web.Repositorios;
public class DniDuplicadoException : Exception;
public class SocioRepositorio(IConfiguration configuration)
{
    private readonly string connectionString = configuration.GetConnectionString("BibliotecaDB")
        ?? throw new InvalidOperationException("Falta ConnectionStrings:BibliotecaDB.");
    public async Task<IEnumerable<Socio>> ListarAsync()
    {
        await using var connection = new SqlConnection(connectionString);
        return await connection.QueryAsync<Socio>("dbo.usp_Socios_Listar", commandType: CommandType.StoredProcedure);
    }
    public async Task CrearAsync(Socio socio)
    {
        await using var connection = new SqlConnection(connectionString);
        try
        {
            await connection.ExecuteAsync("dbo.usp_Socios_Insertar", new { socio.DNI, socio.Nombre, socio.Email }, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException ex) when (ex.Number == 50001)
        {
            throw new DniDuplicadoException();
        }
    }
}
