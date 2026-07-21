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
                .MapGroup("/api/usuario/fotografia")
                .WithTags("Fotografia")
                .RequireCors("ReactDev");

            // PUT / - Actualizar (agregar o reemplazar) fotografía del usuario
            group.MapPut("/", async (
                HttpContext context,
                [FromServices] IFotografiaService service,
                [FromBody] ActualizarFotografiaRequest request) =>
            {
                // --- AUTENTICACIÓN: validación de token contra el método validate del SRV1 ---
                // El token JWT se obtiene del header Authorization: Bearer <token>
                // Se valida contra el endpoint GET /api/auth/validate?token=... del SRV1.
                // Si el token es inválido o está vencido, SRV1 responde 401 y se rechaza la operación.
                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var tokenValidator = context.RequestServices.GetRequiredService<ITokenValidator>();
                if (!await tokenValidator.ValidateAsync(token))
                    return Results.Unauthorized();
                // ------------------------------------------------------------------

                if (string.IsNullOrWhiteSpace(request.UsuarioIdentificacion) ||
                    string.IsNullOrWhiteSpace(request.FotografiaBase64))
                    return Results.BadRequest(new { message = "La identificación y la fotografía son requeridas" });

                var result = await service.ActualizarFotografiaAsync(
                    request.UsuarioIdentificacion, request.FotografiaBase64);

                if (result == -1)
                    return Results.NotFound(new { message = $"El usuario '{request.UsuarioIdentificacion}' no tiene fotografía registrada. Use el endpoint de carga inicial." });
                if (result <= 0)
                    return Results.Problem("No se pudo actualizar la fotografía");

                return Results.Ok(new { message = "Fotografía actualizada correctamente" });
            })
            .WithName("ActualizarFotografia");

            // DELETE /{identificacion} - Eliminar fotografía del usuario
            group.MapDelete("/{identificacion}", async (
                HttpContext context,
                [FromServices] IFotografiaService service,
                string identificacion) =>
            {
                // --- AUTENTICACIÓN: validación de token contra el método validate del SRV1 ---
                // El token JWT se obtiene del header Authorization: Bearer <token>
                // Se valida contra el endpoint GET /api/auth/validate?token=... del SRV1.
                // Si el token es inválido o está vencido, SRV1 responde 401 y se rechaza la operación.
                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var tokenValidator = context.RequestServices.GetRequiredService<ITokenValidator>();
                if (!await tokenValidator.ValidateAsync(token))
                    return Results.Unauthorized();
                // ------------------------------------------------------------------

                if (string.IsNullOrWhiteSpace(identificacion))
                    return Results.BadRequest(new { message = "La identificación es requerida" });

                var result = await service.EliminarFotografiaAsync(identificacion);

                if (result <= 0)
                    return Results.NotFound(new { message = $"No se encontró fotografía para el usuario '{identificacion}'" });

                return Results.Ok(new { message = "Fotografía eliminada correctamente" });
            })
            .WithName("EliminarFotografia");

            // GET /{identificacion} - Obtener fotografía del usuario en Base64
            group.MapGet("/{identificacion}", async (
                HttpContext context,
                [FromServices] IFotografiaService service,
                string identificacion) =>
            {
                // --- AUTENTICACIÓN: validación de token contra el método validate del SRV1 ---
                // El token JWT se obtiene del header Authorization: Bearer <token>
                // Se valida contra el endpoint GET /api/auth/validate?token=... del SRV1.
                // Si el token es inválido o está vencido, SRV1 responde 401 y se rechaza la operación.
                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var tokenValidator = context.RequestServices.GetRequiredService<ITokenValidator>();
                if (!await tokenValidator.ValidateAsync(token))
                    return Results.Unauthorized();
                // ------------------------------------------------------------------

                var foto = await service.ObtenerFotografiaAsync(identificacion);

                return foto is null
                    ? Results.NotFound(new { message = $"No se encontró fotografía para el usuario '{identificacion}'" })
                    : Results.Ok(foto);
            })
            .WithName("ObtenerFotografia");
        }
    }
}
