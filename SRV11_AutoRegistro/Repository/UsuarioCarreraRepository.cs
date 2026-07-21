using SRV11_AutoRegistro.Repository;
using Dapper;

public class UsuarioCarreraRepository
{
    private readonly IDbConnectionFactory _db;

    public UsuarioCarreraRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task AgregarAsync(
        int usuarioId,
        int carreraId)
    {
        using var conn = _db.CreateConnection();

        await conn.ExecuteAsync(
        @"INSERT INTO USUARIO_CARRERA
          (USUARIO_ID, CARRERA_ID)
          VALUES
          (@usuarioId,@carreraId)",
        new
        {
            usuarioId,
            carreraId
        });
    }
}