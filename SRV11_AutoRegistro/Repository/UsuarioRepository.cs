using Dapper;
using SRV11_AutoRegistro.Entities;

namespace SRV11_AutoRegistro.Repository
{
    public class UsuarioRepository
    {
        private readonly IDbConnectionFactory _db;

        public UsuarioRepository(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            using var conn = _db.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<Usuario>(
                @"SELECT *
                  FROM USUARIO
                  WHERE EMAIL = @email",
                new { email });
        }

        public async Task<Usuario?> GetByTokenAsync(string token)
        {
            using var conn = _db.CreateConnection();

            var usuario = await conn.QueryFirstOrDefaultAsync<Usuario>(
                @"
                SELECT
                    ID,
                    EMAIL,
                    CONTRASENA,
                    TIPO_USUARIO_ID AS TipoUsuarioId,
                    TIPO_IDENTIFICACION_ID AS TipoIdentificacionId,
                    NUMERO_IDENTIFICACION AS NumeroIdentificacion,
                    NOMBRE_COMPLETO AS NombreCompleto,
                    ACTIVO,
                    FECHA_CREACION AS FechaCreacion,
                    CONFIRMADO,
                    TOKEN_CONFIRMACION AS TokenConfirmacion,
                    FECHA_EXPIRACION AS FechaExpiracion
                FROM USUARIO
                WHERE TOKEN_CONFIRMACION = @token",
                new { token });

            Console.WriteLine(
                usuario == null
                    ? "NO SE ENCONTRO USUARIO"
                    : $"FECHA = {usuario.FechaExpiracion}");

            return usuario;
        }

        public async Task<int> CreateAsync(Usuario usuario)
        {
            using var conn = _db.CreateConnection();

            var parametros = new
            {
                Email = usuario.Email,
                Contrasena = usuario.Contrasena,
                TipoUsuarioId = usuario.TipoUsuarioId,
                TipoIdentificacionId = usuario.TipoIdentificacionId,
                NumeroIdentificacion = usuario.NumeroIdentificacion,
                NombreCompleto = usuario.NombreCompleto,
                Activo = usuario.Activo,
                FechaCreacion = usuario.FechaCreacion,
                RolId = usuario.RolId,
                Confirmado = usuario.Confirmado,
                TokenConfirmacion = usuario.TokenConfirmacion,
                FechaExpiracion = usuario.FechaExpiracion
            };

            return await conn.ExecuteScalarAsync<int>(
            @"
    INSERT INTO USUARIO
    (
        EMAIL,
        CONTRASENA,
        TIPO_USUARIO_ID,
        TIPO_IDENTIFICACION_ID,
        NUMERO_IDENTIFICACION,
        NOMBRE_COMPLETO,
        ACTIVO,
        FECHA_CREACION,
        ROL_ID,
        CONFIRMADO,
        TOKEN_CONFIRMACION,
        FECHA_EXPIRACION
    )
    OUTPUT INSERTED.ID
    VALUES
    (
        @Email,
        @Contrasena,
        @TipoUsuarioId,
        @TipoIdentificacionId,
        @NumeroIdentificacion,
        @NombreCompleto,
        @Activo,
        @FechaCreacion,
        @RolId,
        @Confirmado,
        @TokenConfirmacion,
        @FechaExpiracion
    )",
            parametros);
        }

        public async Task<int> ConfirmarCuentaAsync(int id)
        {
            using var conn = _db.CreateConnection();

            return await conn.ExecuteAsync(
                @"
                UPDATE USUARIO
                SET
                    CONFIRMADO = 1,
                    TOKEN_CONFIRMACION = NULL,
                    FECHA_EXPIRACION = NULL
                WHERE ID = @id",
                new { id });
        }

        public async Task EliminarUsuarioPendienteAsync(int id)
        {
            using var conn = _db.CreateConnection();

            conn.Open();

            using var transaction = conn.BeginTransaction();

            try
            {
                await conn.ExecuteAsync(
                    @"
            DELETE FROM USUARIO_TELEFONO
            WHERE USUARIO_ID = @id",
                    new { id },
                    transaction);


                await conn.ExecuteAsync(
                    @"
            DELETE FROM USUARIO_CARRERA
            WHERE USUARIO_ID = @id",
                    new { id },
                    transaction);


                await conn.ExecuteAsync(
                    @"
            DELETE FROM USUARIO_AREA
            WHERE USUARIO_ID = @id",
                    new { id },
                    transaction);


                await conn.ExecuteAsync(
                    @"
            DELETE FROM USUARIO_INSTITUCION
            WHERE USUARIO_ID = @id",
                    new { id },
                    transaction);


                await conn.ExecuteAsync(
                    @"
            DELETE FROM USUARIO
            WHERE ID = @id",
                    new { id },
                    transaction);


                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}