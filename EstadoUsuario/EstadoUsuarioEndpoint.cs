using SRV12_EstadoUsuario.Auth;
using SRV12_EstadoUsuario.Entities;
using SRV12_EstadoUsuario.Services;
using Microsoft.AspNetCore.Mvc;

namespace SRV12_EstadoUsuario
{
    public static class EstadoUsuarioEndpoints
    {
        public static void MapEstadoUsuarioEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes
                .MapGroup("/usuarios/estado")
                .WithTags("EstadoUsuario")
                .RequireCors("ReactDev");

            // PATCH / - Cambiar estado de un usuario (SRV12)
            group.MapMethods("/", new[] { "PATCH" }, async (
                HttpContext context,
                [FromServices] IEstadoUsuarioService service,
                [FromBody] CambioEstadoRequest request) =>
            {
                // --- AUTENTICACIÓN: validación de token contra el método validate del SRV1 ---
                // El token JWT se obtiene del header Authorization: Bearer <token>
                // Se valida contra el endpoint GET /api/auth/validate del SRV1.
                // Si el token es inválido o está vencido, SRV1 responde 401 y se rechaza la operación.
                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var tokenValidator = context.RequestServices.GetRequiredService<ITokenValidator>();
                if (!await tokenValidator.ValidateAsync(token))
                    return Results.Unauthorized();
                // ------------------------------------------------------------------

                if (request is null || !request.UsuarioId.HasValue || request.UsuarioId <= 0 ||
                    string.IsNullOrWhiteSpace(request.Estado))
                    return Results.BadRequest(new { message = "El identificador del usuario y el codigo de estado son requeridos" });

                var (result, usuario) = await service.CambiarEstadoAsync(request.UsuarioId.Value, request.Estado);

                if (result == -1)
                    return Results.NotFound(new { message = $"El usuario con identificador {request.UsuarioId} no existe" });
                if (result == -2)
                    return Results.NotFound(new { message = $"El estado '{request.Estado}' no existe" });
                if (result <= 0)
                    return Results.Problem("No se pudo cambiar el estado");

                return Results.Ok(usuario);
            })
            .WithName("CambiarEstadoUsuario");
        }
    }
}
