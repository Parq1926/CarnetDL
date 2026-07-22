using Microsoft.EntityFrameworkCore;
using TipoIdentificacionSRV6.Data;
using TipoIdentificacionSRV6.DTOs;
using TipoIdentificacionSRV6.Entities;

namespace TipoIdentificacionSRV6.Endpoints
{
    public static class TipoIdentificacionEndpoints
    {
        public static void MapTipoIdentificacionEndpoints(this WebApplication app)
        {
            // GET: /api/TipoIdentificacion
            app.MapGet("/api/TipoIdentificacion", GetTipoIdentificaciones);

            // GET: /api/TipoIdentificacion/{id}
            app.MapGet("/api/TipoIdentificacion/{id}", GetTipoIdentificacionById);

            // POST: /api/TipoIdentificacion
            app.MapPost("/api/TipoIdentificacion", CreateTipoIdentificacion);

            // PUT: /api/TipoIdentificacion/{id}
            app.MapPut("/api/TipoIdentificacion/{id}", UpdateTipoIdentificacion);

            // DELETE: /api/TipoIdentificacion/{id}
            app.MapDelete("/api/TipoIdentificacion/{id}", DeleteTipoIdentificacion);

            // GET: /api/TipoIdentificacion/exists/{id}
            app.MapGet("/api/TipoIdentificacion/exists/{id}", ExistsTipoIdentificacion);

            // GET: /api/TipoIdentificacion/exists/nombre/{nombre}
            app.MapGet("/api/TipoIdentificacion/exists/nombre/{nombre}", ExistsTipoIdentificacionByName);
        }

        // ========================================
        // HANDLERS
        // ========================================

        private static async Task<IResult> GetTipoIdentificaciones(ApplicationDbContext db)
        {
            var tipos = await db.TiposIdentificacion
                .Select(t => new TipoIdentificacionDto
                {
                    Id = t.Id,
                    Nombre = t.Nombre
                })
                .ToListAsync();
            return Results.Ok(tipos);
        }

        private static async Task<IResult> GetTipoIdentificacionById(int id, ApplicationDbContext db)
        {
            var tipo = await db.TiposIdentificacion.FindAsync(id);
            if (tipo == null)
                return Results.NotFound(new { message = "Tipo de identificación no encontrado" });

            return Results.Ok(new TipoIdentificacionDto
            {
                Id = tipo.Id,
                Nombre = tipo.Nombre
            });
        }

        private static async Task<IResult> CreateTipoIdentificacion(TipoIdentificacionCreateDto dto, ApplicationDbContext db)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                return Results.BadRequest(new { error = "El nombre es requerido" });

            var tipo = new TipoIdentificacion
            {
                Nombre = dto.Nombre.Trim()
            };

            db.TiposIdentificacion.Add(tipo);
            await db.SaveChangesAsync();

            return Results.Created($"/api/TipoIdentificacion/{tipo.Id}", new TipoIdentificacionDto
            {
                Id = tipo.Id,
                Nombre = tipo.Nombre
            });
        }

        private static async Task<IResult> UpdateTipoIdentificacion(int id, TipoIdentificacionUpdateDto dto, ApplicationDbContext db)
        {
            var tipo = await db.TiposIdentificacion.FindAsync(id);
            if (tipo == null)
                return Results.NotFound(new { message = "Tipo de identificación no encontrado" });

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                return Results.BadRequest(new { error = "El nombre es requerido" });

            tipo.Nombre = dto.Nombre.Trim();
            await db.SaveChangesAsync();

            return Results.Ok(new TipoIdentificacionDto
            {
                Id = tipo.Id,
                Nombre = tipo.Nombre
            });
        }

        private static async Task<IResult> DeleteTipoIdentificacion(int id, ApplicationDbContext db)
        {
            var tipo = await db.TiposIdentificacion.FindAsync(id);
            if (tipo == null)
                return Results.NotFound(new { message = "Tipo de identificación no encontrado" });

            db.TiposIdentificacion.Remove(tipo);
            await db.SaveChangesAsync();

            return Results.NoContent();
        }

        private static async Task<IResult> ExistsTipoIdentificacion(int id, ApplicationDbContext db)
        {
            var exists = await db.TiposIdentificacion.AnyAsync(t => t.Id == id);
            return Results.Ok(new { exists });
        }

        private static async Task<IResult> ExistsTipoIdentificacionByName(string nombre, int? excludeId, ApplicationDbContext db)
        {
            var query = db.TiposIdentificacion.Where(t => t.Nombre == nombre);
            if (excludeId.HasValue)
            {
                query = query.Where(t => t.Id != excludeId.Value);
            }
            var exists = await query.AnyAsync();
            return Results.Ok(new { exists });
        }
    }
}