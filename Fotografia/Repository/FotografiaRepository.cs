using Dapper;

namespace SRV13_Fotografia.Repository
{
    public class FotografiaRepository
    {
        private readonly IDbConnectionFactory _db;

        public FotografiaRepository(IDbConnectionFactory db) { _db = db; }

        public async Task<bool> ExisteUsuarioAsync(int usuarioId)
        {
            using var conn = _db.CreateConnection();
            var id = await conn.QueryFirstOrDefaultAsync<int?>(
                "SELECT Id FROM dbo.Usuario WHERE Id = @usuarioId", new { usuarioId });
            return id.HasValue;
        }

        public async Task<byte[]?> ObtenerAsync(int usuarioId)
        {
            using var conn = _db.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<byte[]?>(
                "SELECT Fotografia FROM dbo.Usuario WHERE Id = @usuarioId", new { usuarioId });
        }

        public async Task<int> ActualizarFotografiaAsync(int usuarioId, byte[] fotografia)
        {
            using var conn = _db.CreateConnection();
            return await conn.ExecuteAsync(
                "UPDATE dbo.Usuario SET Fotografia = @fotografia WHERE Id = @usuarioId",
                new { fotografia, usuarioId });
        }

        public async Task<int> EliminarFotografiaAsync(int usuarioId)
        {
            using var conn = _db.CreateConnection();
            return await conn.ExecuteAsync(
                "UPDATE dbo.Usuario SET Fotografia = NULL WHERE Id = @usuarioId AND Fotografia IS NOT NULL",
                new { usuarioId });
        }
    }
}
