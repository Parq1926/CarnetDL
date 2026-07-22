using Microsoft.AspNetCore.Mvc;
using SRV14_CarnetQR.Auth;
using SRV14_CarnetQR.Services;

namespace SRV14_CarnetQR
{
    public static class CarnetQREndpoints
    {
        public static void MapCarnetQREndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes
                .MapGroup("/usuario/qr")
                .WithTags("CarnetQR")
                .RequireCors("ReactDev");

            // GET /api/usuario/qr?identificacion={id}  (SRV14)
            group.MapGet("/", async (
                HttpContext context,
                [FromServices] ICarnetQRService service,
                [FromQuery] string identificacion) =>
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
                    return Results.BadRequest(new { message = "La identificación del usuario es requerida" });

                var qrBase64 = await service.GenerarQRAsync(identificacion);

                if (qrBase64 is null)
                    return Results.NotFound(new { message = $"No se encontró el usuario con identificación '{identificacion}'" });

                return Results.Ok(qrBase64);
            })
            .WithName("ObtenerCarnetQR")
            .WithSummary("Genera y devuelve el QR del carnet digital para un usuario");
        }
    }
}
