using SRV13_Fotografia.Auth;
using SRV13_Fotografia.Entities;
using SRV13_Fotografia.Services;
using Microsoft.AspNetCore.Mvc;

namespace SRV13_Fotografia
{
    public static class FotografiaEndpoints
    {
        public static void MapFotografiaEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes
                .MapGroup("/usuario/fotografia")
                .WithTags("Fotografia")
                .RequireCors("ReactDev");

            // PUT / - Actualizar (agregar o reemplazar) fotografía del usuario
            group.MapPut("/", async (
                HttpContext context,
                [FromServices] IFotografiaService service,
                [FromBody] ActualizarFotografiaRequest request) =>
            {
                // --- AUTENTICACIÓN: validación de token contra el método validate del SRV1 ---
                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var tokenValidator = context.RequestServices.GetRequiredService<ITokenValidator>();
                if (!await tokenValidator.ValidateAsync(token))
                    return Results.Unauthorized();
                // ------------------------------------------------------------------

                if (request is null || !request.UsuarioId.HasValue || request.UsuarioId <= 0 ||
                    string.IsNullOrWhiteSpace(request.FotografiaBase64))
                    return Results.BadRequest(new { message = "El identificador del usuario y la fotografia son requeridos" });

                var (result, mensaje, fotografia) = await service.ActualizarFotografiaAsync(
                    request.UsuarioId.Value, request.FotografiaBase64);

                if (result == -1)
                    return Results.NotFound(new { message = $"El usuario con identificador {request.UsuarioId} no existe" });
                if (result == -2)
                    return Results.BadRequest(new { message = mensaje });
                if (result <= 0)
                    return Results.Problem("No se pudo actualizar la fotografia");

                return Results.Ok(fotografia);
            })
            .WithName("ActualizarFotografia");

            // DELETE /{usuarioId} - Eliminar fotografía del usuario
            group.MapDelete("/{usuarioId:int}", async (
                HttpContext context,
                [FromServices] IFotografiaService service,
                int usuarioId) =>
            {
                // --- AUTENTICACIÓN: validación de token contra el método validate del SRV1 ---
                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var tokenValidator = context.RequestServices.GetRequiredService<ITokenValidator>();
                if (!await tokenValidator.ValidateAsync(token))
                    return Results.Unauthorized();
                // ------------------------------------------------------------------

                if (usuarioId <= 0)
                    return Results.BadRequest(new { message = "El identificador del usuario es requerido" });

                var (result, fotografia) = await service.EliminarFotografiaAsync(usuarioId);

                if (result == -1)
                    return Results.NotFound(new { message = $"El usuario con identificador {usuarioId} no existe" });
                if (result <= 0)
                    return Results.NotFound(new { message = $"El usuario con identificador {usuarioId} no tiene fotografia registrada" });

                return Results.Ok(fotografia);
            })
            .WithName("EliminarFotografia");

            // GET /{usuarioId} - Obtener fotografía del usuario en Base 64
            group.MapGet("/{usuarioId:int}", async (
                HttpContext context,
                [FromServices] IFotografiaService service,
                int usuarioId) =>
            {
                // --- AUTENTICACIÓN: validación de token contra el método validate del SRV1 ---
                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var tokenValidator = context.RequestServices.GetRequiredService<ITokenValidator>();
                if (!await tokenValidator.ValidateAsync(token))
                    return Results.Unauthorized();
                // ------------------------------------------------------------------

                if (usuarioId <= 0)
                    return Results.BadRequest(new { message = "El identificador del usuario es requerido" });

                var foto = await service.ObtenerFotografiaAsync(usuarioId);

                return foto is null
                    ? Results.NotFound(new { message = $"No se encontro fotografia para el usuario con identificador {usuarioId}" })
                    : Results.Ok(foto);
            })
            .WithName("ObtenerFotografia");
        }
    }
}
