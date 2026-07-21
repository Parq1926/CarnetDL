using SRV11_AutoRegistro.Entities;
using SRV11_AutoRegistro.Services;

namespace SRV11_AutoRegistro
{
    public static class AutoRegistroEndpoints
    {
        public static void MapAutoRegistroEndpoints(this WebApplication app)
        {
            app.MapPost("/autoregistro",
            async (
                Usuario usuario,
                IUsuarioService service) =>
            {
                var resultado = await service.RegistrarAsync(usuario);

                if (!resultado.ok)
                    return Results.BadRequest(new
                    {
                        mensaje = resultado.error
                    });

                return Results.Ok(new
                {
                    mensaje = "Usuario registrado correctamente. Revise su correo para confirmar la cuenta.",

                    id = resultado.usuarioCreado!.ID,
                    email = resultado.usuarioCreado.Email,
                    nombreCompleto = resultado.usuarioCreado.NombreCompleto,
                    numeroIdentificacion = resultado.usuarioCreado.NumeroIdentificacion,
                    tipoUsuarioId = resultado.usuarioCreado.TipoUsuarioId,
                    tipoIdentificacionId = resultado.usuarioCreado.TipoIdentificacionId,
                    instituciones = resultado.usuarioCreado.Instituciones,
                    carrerasAsociadas = resultado.usuarioCreado.CarrerasAsociadas,
                    areasAsociadas = resultado.usuarioCreado.AreasAsociadas,
                    telefonos = resultado.usuarioCreado.Telefonos,
                    activo = resultado.usuarioCreado.Activo,
                    confirmado = resultado.usuarioCreado.Confirmado,
                    fechaCreacion = resultado.usuarioCreado.FechaCreacion
                });
            });

            app.MapGet("/autoregistro/confirmar/{token}",
            async (
                string token,
                IUsuarioService service) =>
            {
                var resultado =
                    await service.ConfirmarCuentaAsync(token);

                if (!resultado.ok)
                    return Results.BadRequest(new
                    {
                        mensaje = resultado.error
                    });

                return Results.Ok(new
                {
                    mensaje = "Cuenta confirmada correctamente"
                });
            });
        }
    }
}