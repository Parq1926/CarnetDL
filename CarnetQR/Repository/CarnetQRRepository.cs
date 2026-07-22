using Dapper;
using SRV14_CarnetQR.Entities;

namespace SRV14_CarnetQR.Repository
{
    public class CarnetQRRepository
    {
        private readonly IDbConnectionFactory _db;
        public CarnetQRRepository(IDbConnectionFactory db) { _db = db; }

        public async Task<UsuarioCarnet?> ObtenerUsuarioAsync(string identificacion)
        {
            using var conn = _db.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<UsuarioCarnet>(
                @"SELECT u.Id                   AS Id,
                         u.NombreCompleto       AS NombreCompleto,
                         u.NumeroIdentificacion AS NumeroIdentificacion,
                         tu.Nombre              AS TipoUsuario
                  FROM dbo.Usuario u
                       INNER JOIN dbo.TipoUsuario tu ON tu.Id = u.TipoUsuarioId
                  WHERE u.NumeroIdentificacion = @identificacion",
                new { identificacion = identificacion.Trim() });
        }

        // Carreras asociadas (estudiantes)
        public async Task<List<string>> ObtenerCarrerasAsync(int usuarioId)
        {
            using var conn = _db.CreateConnection();
            var datos = await conn.QueryAsync<string>(
                "SELECT CarreraId FROM dbo.UsuarioCarrera WHERE UsuarioId = @usuarioId ORDER BY CarreraId",
                new { usuarioId });
            return datos.ToList();
        }

        // Areas asociadas (funcionarios)
        public async Task<List<string>> ObtenerAreasAsync(int usuarioId)
        {
            using var conn = _db.CreateConnection();
            var datos = await conn.QueryAsync<string>(
                "SELECT AreaId FROM dbo.UsuarioArea WHERE UsuarioId = @usuarioId ORDER BY AreaId",
                new { usuarioId });
            return datos.ToList();
        }

        public async Task<List<string>> ObtenerInstitucionesAsync(int usuarioId)
        {
            using var conn = _db.CreateConnection();
            var datos = await conn.QueryAsync<string>(
                "SELECT InstitucionId FROM dbo.UsuarioInstitucion WHERE UsuarioId = @usuarioId ORDER BY InstitucionId",
                new { usuarioId });
            return datos.ToList();
        }
    }
}
