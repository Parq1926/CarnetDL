using Dapper;

namespace SRV11_AutoRegistro.Repository
{
    public class UsuarioInstitucionRepository
    {
        private readonly IDbConnectionFactory _db;

        public UsuarioInstitucionRepository(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task AgregarAsync(int usuarioId, int institucionId)
        {
            using var conn = _db.CreateConnection();

            await conn.ExecuteAsync(
            @"
            INSERT INTO USUARIO_INSTITUCION
            (
                USUARIO_ID,
                INSTITUCION_ID
            )
            VALUES
            (
                @usuarioId,
                @institucionId
            )",
            new
            {
                usuarioId,
                institucionId
            });
        }
    }
}