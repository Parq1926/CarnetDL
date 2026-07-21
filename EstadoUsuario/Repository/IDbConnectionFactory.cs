using System.Data;

namespace SRV12_EstadoUsuario.Repository
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
