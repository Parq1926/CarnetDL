using Dapper;
using SRV12_EstadoUsuario.Entities;

namespace SRV12_EstadoUsuario.Repository
{
    public class EstadoUsuarioRepository
    {
        private readonly IDbConnectionFactory _db;
        public EstadoUsuarioRepository(IDbConnectionFactory db) { _db = db; }

        // El estado del usuario es la columna dbo.Usuario.EstadoId,
        // que referencia al catalogo dbo.EstadoUsuario (Id, Nombre)
        public async Task<EstadoUsuario?> GetEstadoAsync(string codigoEstado)
        {
            using var conn = _db.CreateConnection();

            if (int.TryParse(codigoEstado, out var id))
            {
                var porId = await conn.QueryFirstOrDefaultAsync<EstadoUsuario>(
                    "SELECT Id, Nombre FROM dbo.EstadoUsuario WHERE Id = @id", new { id });

                if (porId is not null) return porId;
            }

            return await conn.QueryFirstOrDefaultAsync<EstadoUsuario>(
                "SELECT Id, Nombre FROM dbo.EstadoUsuario WHERE LTRIM(RTRIM(Nombre)) = @nombre",
                new { nombre = codigoEstado.Trim() });
        }

        public async Task<bool> ExisteUsuarioAsync(int usuarioId)
        {
            using var conn = _db.CreateConnection();
            var id = await conn.QueryFirstOrDefaultAsync<int?>(
                "SELECT Id FROM dbo.Usuario WHERE Id = @usuarioId", new { usuarioId });
            return id.HasValue;
        }

        public async Task<UsuarioEstado?> GetUsuarioEstadoAsync(int usuarioId)
        {
            using var conn = _db.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<UsuarioEstado>(
                @"SELECT u.Id             AS UsuarioId,
                         u.NombreCompleto AS NombreCompleto,
                         u.EstadoId       AS EstadoId,
                         e.Nombre         AS Estado
                  FROM dbo.Usuario u
                       INNER JOIN dbo.EstadoUsuario e ON e.Id = u.EstadoId
                  WHERE u.Id = @usuarioId",
                new { usuarioId });
        }

        public async Task<int> CambiarEstadoAsync(int usuarioId, int estadoId)
        {
            using var conn = _db.CreateConnection();
            return await conn.ExecuteAsync(
                "UPDATE dbo.Usuario SET EstadoId = @estadoId WHERE Id = @usuarioId",
                new { estadoId, usuarioId });
        }
    }
}
