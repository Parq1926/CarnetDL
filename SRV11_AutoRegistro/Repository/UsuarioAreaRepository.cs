using Dapper;

namespace SRV11_AutoRegistro.Repository
{
    public class UsuarioAreaRepository
    {
        private readonly IDbConnectionFactory _db;

        public UsuarioAreaRepository(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task AgregarAsync(int usuarioId, int areaId)
        {
            using var conn = _db.CreateConnection();

            await conn.ExecuteAsync(
            @"
            INSERT INTO USUARIO_AREA
            (
                USUARIO_ID,
                AREA_ID
            )
            VALUES
            (
                @usuarioId,
                @areaId
            )",
            new
            {
                usuarioId,
                areaId
            });
        }
    }
}