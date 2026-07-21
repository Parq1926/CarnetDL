using System.Data;

namespace SRV11_AutoRegistro.Repository
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}