using Dapper;
using SRV13_Fotografia.Entities;

namespace SRV13_Fotografia.Repository
{
    public class FotografiaRepository
    {
        private readonly IDbConnectionFactory _db;

        public FotografiaRepository(IDbConnectionFactory db) { _db = db; }

        public async Task<FotografiaUsuario?> ObtenerPorIdentificacionAsync(string identificacion)
        {
            using var conn = _db.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<FotografiaUsuario>(
                @"SELECT ID AS Id,
                         USUARIO_IDENTIFICACION AS UsuarioIdentificacion,
                         FOTOGRAFIA_BASE64 AS FotografiaBase64,
                         FECHA_CREACION AS FechaCreacion,
                         FECHA_MODIFICACION AS FechaModificacion
                  FROM FOTOGRAFIAUSUARIO
                  WHERE USUARIO_IDENTIFICACION = @identificacion",
                new { identificacion });
        }

        public async Task<int> ActualizarFotografiaAsync(string identificacion, string fotografiaBase64)
        {
            using var conn = _db.CreateConnection();

            var existe = await conn.QueryFirstOrDefaultAsync<int?>(
                "SELECT ID FROM FOTOGRAFIAUSUARIO WHERE USUARIO_IDENTIFICACION = @identificacion",
                new { identificacion });

            if (!existe.HasValue)
                return -1; // Usuario no tiene fotografía registrada

            return await conn.ExecuteAsync(
                @"UPDATE FOTOGRAFIAUSUARIO
                  SET FOTOGRAFIA_BASE64 = @fotografiaBase64,
                      FECHA_MODIFICACION = GETDATE()
                  WHERE USUARIO_IDENTIFICACION = @identificacion",
                new { fotografiaBase64, identificacion });
        }

        public async Task<int> EliminarFotografiaAsync(string identificacion)
        {
            using var conn = _db.CreateConnection();
            return await conn.ExecuteAsync(
                "DELETE FROM FOTOGRAFIAUSUARIO WHERE USUARIO_IDENTIFICACION = @identificacion",
                new { identificacion });
        }
    }
}
