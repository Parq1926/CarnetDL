using Dapper;

namespace SRV11_AutoRegistro.Repository
{
    public class UsuarioTelefonoRepository
    {
        private readonly IDbConnectionFactory _db;

        public UsuarioTelefonoRepository(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task AgregarAsync(
            int usuarioId,
            string telefono)
        {
            using var conn = _db.CreateConnection();

            await conn.ExecuteAsync(
                @"INSERT INTO USUARIO_TELEFONO
                  (
                    USUARIO_ID,
                    TELEFONO
                  )
                  VALUES
                  (
                    @usuarioId,
                    @telefono
                  )",
                new
                {
                    usuarioId,
                    telefono
                });
        }
    }
}