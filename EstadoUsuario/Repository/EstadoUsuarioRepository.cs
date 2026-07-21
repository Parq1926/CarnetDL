using Dapper;
using SRV12_EstadoUsuario.Entities;

namespace SRV12_EstadoUsuario.Repository
{
    public class EstadoUsuarioRepository
    {
        private readonly IDbConnectionFactory _db;
        public EstadoUsuarioRepository(IDbConnectionFactory db) { _db = db; }

        public async Task<IEnumerable<EstadoUsuario>> GetAllEstadosAsync()
        {
            using var conn = _db.CreateConnection();
            return await conn.QueryAsync<EstadoUsuario>(
                "SELECT ID AS Id, CODIGO AS Codigo, DESCRIPCION AS Descripcion FROM ESTADOUSUARIO");
        }

        public async Task<EstadoUsuario?> GetEstadoByCodigoAsync(string codigo)
        {
            using var conn = _db.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<EstadoUsuario>(
                "SELECT ID AS Id, CODIGO AS Codigo, DESCRIPCION AS Descripcion FROM ESTADOUSUARIO WHERE CODIGO = @codigo",
                new { codigo });
        }

        public async Task<UsuarioEstado?> GetEstadoUsuarioAsync(string identificacion)
        {
            using var conn = _db.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<UsuarioEstado>(
                @"SELECT ID AS Id, USUARIO_IDENTIFICACION AS UsuarioIdentificacion, 
                  ESTADOUSUARIO_ID AS EstadoUsuarioId, FECHA_MODIFICACION AS FechaModificacion 
                  FROM USUARIOESTADO WHERE USUARIO_IDENTIFICACION = @identificacion",
                new { identificacion });
        }

        public async Task<int> UpsertEstadoUsuarioAsync(string identificacion, int estadoId)
        {
            using var conn = _db.CreateConnection();
            var exists = await conn.QueryFirstOrDefaultAsync<int?>(
                "SELECT ID FROM USUARIOESTADO WHERE USUARIO_IDENTIFICACION = @identificacion",
                new { identificacion });

            if (exists.HasValue)
                return await conn.ExecuteAsync(
                    @"UPDATE USUARIOESTADO SET ESTADOUSUARIO_ID = @estadoId, 
                      FECHA_MODIFICACION = GETDATE() WHERE USUARIO_IDENTIFICACION = @identificacion",
                    new { estadoId, identificacion });

            return await conn.ExecuteAsync(
                "INSERT INTO USUARIOESTADO (USUARIO_IDENTIFICACION, ESTADOUSUARIO_ID) VALUES (@identificacion, @estadoId)",
                new { identificacion, estadoId });
        }
    }
}
