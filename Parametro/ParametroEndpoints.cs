using Microsoft.AspNetCore.Mvc;
using SRV15_Parametro.Auth;
using SRV15_Parametro.Entities;
using SRV15_Parametro.Services;

namespace SRV15_Parametro
{
    public static class ParametroEndpoints
    {
        public static void MapParametroEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes
                .MapGroup("/parametro")
                .WithTags("Parametro")
                .RequireCors("ReactDev");

            // GET /api/parametro - Obtener todos
            group.MapGet("/", async (
                HttpContext context,
                [FromServices] IParametroService service) =>
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

                var lista = await service.GetAllAsync();
                return Results.Ok(lista);
            })
            .WithName("GetAllParametros");

            // GET /api/parametro/{id} - Obtener por ID
            group.MapGet("/{id}", async (
                HttpContext context,
                string id,
                [FromServices] IParametroService service) =>
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

                if (string.IsNullOrWhiteSpace(id))
                    return Results.BadRequest(new { message = "El identificador es requerido" });

                var parametro = await service.GetByIdAsync(id);
                return parametro is null
                    ? Results.NotFound(new { message = $"No se encontró el parámetro '{id}'" })
                    : Results.Ok(parametro);
            })
            .WithName("GetParametroById");

            // POST /api/parametro - Crear
            group.MapPost("/", async (
                HttpContext context,
                [FromBody] ParametroRequest request,
                [FromServices] IParametroService service) =>
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

                var (ok, error) = await service.CreateAsync(request);

                if (!ok && error.Contains("Ya existe"))
                    return Results.Conflict(new { message = error });

                if (!ok)
                    return Results.BadRequest(new { message = error });

                var creado = await service.GetByIdAsync(request.Id);
                return Results.Created($"/api/parametro/{request.Id}", creado);
            })
            .WithName("CreateParametro");

            // PUT /api/parametro/{id} - Modificar
            group.MapPut("/{id}", async (
                HttpContext context,
                string id,
                [FromBody] ParametroRequest request,
                [FromServices] IParametroService service) =>
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

                if (string.IsNullOrWhiteSpace(id))
                    return Results.BadRequest(new { message = "El identificador es requerido" });

                var (ok, error) = await service.UpdateAsync(id, request);
                if (!ok && error.Contains("No se encontró"))
                    return Results.NotFound(new { message = error });

                if (!ok)
                    return Results.BadRequest(new { message = error });

                var actualizado = await service.GetByIdAsync(id);
                return Results.Ok(actualizado);
            })
            .WithName("UpdateParametro");

            // DELETE /api/parametro/{id} - Eliminar
            group.MapDelete("/{id}", async (
                HttpContext context,
                string id,
                [FromServices] IParametroService service) =>
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

                if (string.IsNullOrWhiteSpace(id))
                    return Results.BadRequest(new { message = "El identificador es requerido" });

                var eliminado = await service.GetByIdAsync(id);

                var (ok, error) = await service.DeleteAsync(id);
                return ok
                    ? Results.Ok(eliminado)
                    : Results.NotFound(new { message = error });
            })
            .WithName("DeleteParametro");
        }
    }
}
