using Dapper;
using SRV15_Parametro.Entities;

namespace SRV15_Parametro.Repository
{
    public class ParametroRepository
    {
        private readonly IDbConnectionFactory _db;
        public ParametroRepository(IDbConnectionFactory db) { _db = db; }

        public async Task<IEnumerable<Parametro>> GetAllAsync()
        {
            using var conn = _db.CreateConnection();
            return await conn.QueryAsync<Parametro>(
                "SELECT ID AS Id, VALOR AS Valor FROM PARAMETRO");
        }

        public async Task<Parametro?> GetByIdAsync(string id)
        {
            using var conn = _db.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Parametro>(
                "SELECT ID AS Id, VALOR AS Valor FROM PARAMETRO WHERE ID = @id",
                new { id });
        }

        public async Task<int> CreateAsync(Parametro parametro)
        {
            using var conn = _db.CreateConnection();
            return await conn.ExecuteAsync(
                "INSERT INTO PARAMETRO (ID, VALOR) VALUES (@Id, @Valor)",
                parametro);
        }

        public async Task<int> UpdateAsync(Parametro parametro)
        {
            using var conn = _db.CreateConnection();
            return await conn.ExecuteAsync(
                "UPDATE PARAMETRO SET VALOR = @Valor WHERE ID = @Id",
                parametro);
        }

        public async Task<int> DeleteAsync(string id)
        {
            using var conn = _db.CreateConnection();
            return await conn.ExecuteAsync(
                "DELETE FROM PARAMETRO WHERE ID = @id",
                new { id });
        }
    }
}
