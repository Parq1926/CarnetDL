using System.Data;

namespace SRV4_Areas.Repository;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}