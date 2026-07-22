using Dapper;
using SRV3_Carreras.Entities;

namespace SRV3_Carreras.Repository;

public interface ICarreraRepository
{
    Task<IEnumerable<Carrera>> GetAll();
    Task<Carrera?> GetById(int id);
    Task<int> Create(Carrera carrera);
    Task<bool> Update(Carrera carrera);
    Task<bool> Delete(int id);
    Task<bool> ExistsByNombre(string nombre, int? excludeId = null);
}

public class CarreraRepository : ICarreraRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public CarreraRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Carrera>> GetAll()
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Carrera WHERE Activo = 1 ORDER BY ID";
        return await connection.QueryAsync<Carrera>(sql);
    }

    public async Task<Carrera?> GetById(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Carrera WHERE ID = @Id AND Activo = 1";
        return await connection.QueryFirstOrDefaultAsync<Carrera>(sql, new { Id = id });
    }

    public async Task<int> Create(Carrera carrera)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = @"
            INSERT INTO Carrera (Nombre, Director, Email, Telefono, InstitucionID, InstitucionNombre, Activo, FechaCreacion)
            VALUES (@Nombre, @Director, @Email, @Telefono, @InstitucionID, @InstitucionNombre, 1, GETDATE());
            SELECT CAST(SCOPE_IDENTITY() as int)";
        return await connection.QuerySingleAsync<int>(sql, carrera);
    }

    public async Task<bool> Update(Carrera carrera)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = @"
            UPDATE Carrera 
            SET Nombre = @Nombre, Director = @Director, Email = @Email, Telefono = @Telefono, 
                InstitucionID = @InstitucionID, InstitucionNombre = @InstitucionNombre, FechaModificacion = GETDATE()
            WHERE ID = @ID AND Activo = 1";
        var rows = await connection.ExecuteAsync(sql, carrera);
        return rows > 0;
    }

    public async Task<bool> Delete(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "UPDATE Carrera SET Activo = 0 WHERE ID = @Id";
        var rows = await connection.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }

    public async Task<bool> ExistsByNombre(string nombre, int? excludeId = null)
    {
        using var connection = _connectionFactory.CreateConnection();
        string sql = "SELECT COUNT(1) FROM Carrera WHERE Nombre = @Nombre AND Activo = 1";
        if (excludeId.HasValue)
            sql += " AND ID != @ExcludeId";

        var count = await connection.ExecuteScalarAsync<int>(sql, new { Nombre = nombre, ExcludeId = excludeId });
        return count > 0;
    }
}