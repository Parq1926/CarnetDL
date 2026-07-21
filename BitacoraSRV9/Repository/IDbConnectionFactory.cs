using Microsoft.Data.SqlClient;
using System.Data;

namespace BitacoraSRV9.Repository;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly IConfiguration _configuration;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection CreateConnection()
    {
        var connectionString =
            _configuration.GetConnectionString("DefaultConnection");

        Console.WriteLine("================================");
        Console.WriteLine("CONNECTION STRING UTILIZADA:");
        Console.WriteLine(connectionString);
        Console.WriteLine("================================");

        return new SqlConnection(connectionString);
    }
}