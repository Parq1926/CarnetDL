using Microsoft.AspNetCore.Mvc;
using UsuariosSRV4.DTOs;
using UsuariosSRV4.Services;

namespace UsuariosSRV4.Endpoints
{
    public static class UsuarioEndpoints
    {
        public static void MapUsuarioEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/Usuarios");

            group.MapGet("/", GetAllAsync);
            group.MapGet("/{id}", GetByIdAsync);
            group.MapPost("/", CreateAsync);
            group.MapPut("/{id}", UpdateAsync);
            group.MapDelete("/{id}", DeleteAsync);

            // ✅ Endpoint para validar credenciales
            group.MapPost("/validar-credenciales", ValidarCredencialesAsync);
        }

        private static async Task<IResult> GetAllAsync(IUsuarioService service)
        {
            var (ok, error, data) = await service.GetAllAsync();
            if (!ok) return Results.BadRequest(new { error });
            return Results.Ok(data);
        }

        private static async Task<IResult> GetByIdAsync(int id, IUsuarioService service)
        {
            var (ok, error, data) = await service.GetByIdAsync(id);
            if (!ok) return Results.NotFound(new { error });
            return Results.Ok(data);
        }

        private static async Task<IResult> CreateAsync([FromBody] CrearUsuarioDto dto, IUsuarioService service)
        {
            var (ok, error, data) = await service.CreateAsync(dto);
            if (!ok) return Results.BadRequest(new { error });
            return Results.Created($"/api/Usuarios/{data.Id}", data);
        }

        private static async Task<IResult> UpdateAsync(int id, [FromBody] ActualizarUsuarioDto dto, IUsuarioService service)
        {
            if (id != dto.Id) return Results.BadRequest(new { error = "El ID no coincide" });
            var (ok, error, data) = await service.UpdateAsync(id, dto);
            if (!ok) return Results.BadRequest(new { error });
            return Results.Ok(data);
        }

        private static async Task<IResult> DeleteAsync(int id, IUsuarioService service)
        {
            var (ok, error) = await service.DeleteAsync(id);
            if (!ok) return Results.BadRequest(new { error });
            return Results.Ok(new { message = "Usuario eliminado correctamente" });
        }

        private static async Task<IResult> ValidarCredencialesAsync(
            [FromBody] ValidarCredencialesRequest request,
            IUsuarioService service)
        {
            var (ok, error, data) = await service.ValidarCredencialesAsync(
                request.Email,
                request.Password,
                request.Tipo);

            if (!ok || data == null)
            {
                return Results.NotFound(new { message = error ?? "Credenciales inválidas" });
            }

            return Results.Ok(data);
        }
    }
}