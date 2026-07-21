using System.Data;

namespace SRV2_Instituciones.Repository;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}