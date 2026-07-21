using Dapper;
using RolSRV8.Entities;

namespace RolSRV8.Repository;

public class RolRepository
{
    private readonly IDbConnectionFactory _db;

    public RolRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Rol>> ObtenerTodosAsync()
    {
        using var conn = _db.CreateConnection();

        return await conn.QueryAsync<Rol>(
        @"
        SELECT
            ID AS Id,
            NOMBRE AS Nombre,
            PANTALLAS AS Pantallas
        FROM ROL
        ORDER BY ID");
    }

    public async Task<Rol?> ObtenerPorIdAsync(int id)
    {
        using var conn = _db.CreateConnection();

        return await conn.QueryFirstOrDefaultAsync<Rol>(
        @"
        SELECT
            ID AS Id,
            NOMBRE AS Nombre,
            PANTALLAS AS Pantallas
        FROM ROL
        WHERE ID = @id",
        new { id });
    }

    public async Task<int> CrearAsync(Rol rol)
    {
        using var conn = _db.CreateConnection();

        return await conn.ExecuteAsync(
        @"
        INSERT INTO ROL
        (
            NOMBRE,
            PANTALLAS
        )
        VALUES
        (
            @Nombre,
            @Pantallas
        )",
        rol);
    }

    public async Task<int> ActualizarAsync(Rol rol)
    {
        using var conn = _db.CreateConnection();

        return await conn.ExecuteAsync(
        @"
        UPDATE ROL
        SET
            NOMBRE = @Nombre,
            PANTALLAS = @Pantallas
        WHERE ID = @Id",
        rol);
    }

    public async Task<int> EliminarAsync(int id)
    {
        using var conn = _db.CreateConnection();

        return await conn.ExecuteAsync(
        @"
        DELETE FROM ROL
        WHERE ID = @id",
        new { id });
    }

    public async Task<bool> TieneUsuariosAsync(int id)
    {
        using var conn = _db.CreateConnection();

        var cantidad = await conn.ExecuteScalarAsync<int>(
        @"
    SELECT COUNT(*)
    FROM USUARIO
    WHERE ROL_ID = @id",
        new { id });

        return cantidad > 0;
    }

    public async Task<int> ContarUsuariosAsync(int id)
    {
        using var conn = _db.CreateConnection();

        return await conn.ExecuteScalarAsync<int>(
        @"
    SELECT COUNT(*)
    FROM USUARIO
    WHERE ROL_ID = @id",
        new { id });
    }
}