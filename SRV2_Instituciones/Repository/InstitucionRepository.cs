using Dapper;
using SRV2_Instituciones.Entities;

namespace SRV2_Instituciones.Repository;

public interface IInstitucionRepository
{
    Task<IEnumerable<Institucion>> GetAll();
    Task<Institucion?> GetById(int id);
    Task<int> Create(Institucion institucion);
    Task<bool> Update(Institucion institucion);
    Task<bool> Delete(int id);
    Task<bool> ExistsByNombre(string nombre, int? excludeId = null);
}

public class InstitucionRepository : IInstitucionRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public InstitucionRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Institucion>> GetAll()
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Institucion WHERE Activo = 1 ORDER BY ID";
        return await connection.QueryAsync<Institucion>(sql);
    }

    public async Task<Institucion?> GetById(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Institucion WHERE ID = @Id AND Activo = 1";
        return await connection.QueryFirstOrDefaultAsync<Institucion>(sql, new { Id = id });
    }

    public async Task<int> Create(Institucion institucion)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = @"
            INSERT INTO Institucion (Nombre, Email, Telefono, Dominios, Activo, FechaCreacion)
            VALUES (@Nombre, @Email, @Telefono, @Dominios, 1, GETDATE());
            SELECT CAST(SCOPE_IDENTITY() as int)";
        return await connection.QuerySingleAsync<int>(sql, institucion);
    }

    public async Task<bool> Update(Institucion institucion)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = @"
            UPDATE Institucion 
            SET Nombre = @Nombre, Email = @Email, Telefono = @Telefono, Dominios = @Dominios, FechaModificacion = GETDATE()
            WHERE ID = @ID AND Activo = 1";
        var rows = await connection.ExecuteAsync(sql, institucion);
        return rows > 0;
    }

    public async Task<bool> Delete(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "UPDATE Institucion SET Activo = 0 WHERE ID = @Id";
        var rows = await connection.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }

    public async Task<bool> ExistsByNombre(string nombre, int? excludeId = null)
    {
        using var connection = _connectionFactory.CreateConnection();
        string sql = "SELECT COUNT(1) FROM Institucion WHERE Nombre = @Nombre AND Activo = 1";
        if (excludeId.HasValue)
            sql += " AND ID != @ExcludeId";

        var count = await connection.ExecuteScalarAsync<int>(sql, new { Nombre = nombre, ExcludeId = excludeId });
        return count > 0;
    }
}