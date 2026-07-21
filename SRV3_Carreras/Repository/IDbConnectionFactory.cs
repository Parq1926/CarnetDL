using System.Data;

namespace SRV3_Carreras.Repository;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}