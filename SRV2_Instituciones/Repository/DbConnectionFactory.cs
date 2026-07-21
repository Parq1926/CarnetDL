using System.Data;
using Microsoft.Data.SqlClient;

namespace SRV2_Instituciones.Repository;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new Exception("Connection string no encontrada");
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}