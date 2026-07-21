using System.Data;

namespace SRV13_Fotografia.Repository
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
