using Dapper;
using SRV14_CarnetQR.Entities;

namespace SRV14_CarnetQR.Repository
{
    public class CarnetQRRepository
    {
        private readonly IDbConnectionFactory _db;
        public CarnetQRRepository(IDbConnectionFactory db) { _db = db; }

        public async Task<int> UpsertCarnetQRAsync(string identificacion, string qrBase64)
        {
            using var conn = _db.CreateConnection();

            var exists = await conn.QueryFirstOrDefaultAsync<int?>(
                "SELECT ID FROM CARNETQR WHERE USUARIO_IDENTIFICACION = @identificacion",
                new { identificacion });

            if (exists.HasValue)
                return await conn.ExecuteAsync(
                    @"UPDATE CARNETQR 
                      SET QR_BASE64 = @qrBase64, FECHA_GENERACION = GETDATE()
                      WHERE USUARIO_IDENTIFICACION = @identificacion",
                    new { qrBase64, identificacion });

            return await conn.ExecuteAsync(
                @"INSERT INTO CARNETQR (USUARIO_IDENTIFICACION, QR_BASE64)
                  VALUES (@identificacion, @qrBase64)",
                new { identificacion, qrBase64 });
        }

        public async Task<CarnetQR?> GetByIdentificacionAsync(string identificacion)
        {
            using var conn = _db.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<CarnetQR>(
                @"SELECT ID AS Id,
                         USUARIO_IDENTIFICACION AS UsuarioIdentificacion,
                         QR_BASE64             AS QrBase64,
                         FECHA_GENERACION      AS FechaGeneracion
                  FROM CARNETQR
                  WHERE USUARIO_IDENTIFICACION = @identificacion",
                new { identificacion });
        }
    }
}
