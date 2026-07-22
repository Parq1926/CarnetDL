using Dapper;
using SRV4_Areas.Entities;

namespace SRV4_Areas.Repository;

public interface IAreaRepository
{
    Task<IEnumerable<AreaTrabajo>> GetAll();
    Task<AreaTrabajo?> GetById(int id);
    Task<int> Create(AreaTrabajo area);
    Task<bool> Update(AreaTrabajo area);
    Task<bool> Delete(int id);
    Task<bool> ExistsByNombre(string nombre, int? excludeId = null);
}

public class AreaRepository : IAreaRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public AreaRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<AreaTrabajo>> GetAll()
    {
        using var connection = _connectionFactory.CreateConnection();
        // ✅ Usar dbo explícitamente
        const string sql = "SELECT * FROM dbo.AreaTrabajo WHERE Activo = 1 ORDER BY ID";
        return await connection.QueryAsync<AreaTrabajo>(sql);
    }

    public async Task<AreaTrabajo?> GetById(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "SELECT * FROM dbo.AreaTrabajo WHERE ID = @Id AND Activo = 1";
        return await connection.QueryFirstOrDefaultAsync<AreaTrabajo>(sql, new { Id = id });
    }

    public async Task<int> Create(AreaTrabajo area)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = @"
            INSERT INTO dbo.AreaTrabajo (Nombre, InstitucionID, InstitucionNombre, Activo, FechaCreacion)
            VALUES (@Nombre, @InstitucionID, @InstitucionNombre, 1, GETDATE());
            SELECT CAST(SCOPE_IDENTITY() as int)";
        return await connection.QuerySingleAsync<int>(sql, area);
    }

    public async Task<bool> Update(AreaTrabajo area)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = @"
            UPDATE dbo.AreaTrabajo 
            SET Nombre = @Nombre, InstitucionID = @InstitucionID, InstitucionNombre = @InstitucionNombre, FechaModificacion = GETDATE()
            WHERE ID = @ID AND Activo = 1";
        var rows = await connection.ExecuteAsync(sql, area);
        return rows > 0;
    }

    public async Task<bool> Delete(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "UPDATE dbo.AreaTrabajo SET Activo = 0 WHERE ID = @Id";
        var rows = await connection.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }

    public async Task<bool> ExistsByNombre(string nombre, int? excludeId = null)
    {
        using var connection = _connectionFactory.CreateConnection();
        string sql = "SELECT COUNT(1) FROM dbo.AreaTrabajo WHERE Nombre = @Nombre AND Activo = 1";
        if (excludeId.HasValue)
            sql += " AND ID != @ExcludeId";

        var count = await connection.ExecuteScalarAsync<int>(sql, new { Nombre = nombre, ExcludeId = excludeId });
        return count > 0;
    }
}