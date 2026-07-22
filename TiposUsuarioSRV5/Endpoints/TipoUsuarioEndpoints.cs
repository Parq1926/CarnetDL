using Microsoft.EntityFrameworkCore;
using TiposUsuarioSRV5.Data;
using TiposUsuarioSRV5.DTOs;
using TiposUsuarioSRV5.Entities;

namespace TiposUsuarioSRV5.Endpoints
{
    public static class TipoUsuarioEndpoints
    {
        public static void MapTipoUsuarioEndpoints(this WebApplication app)
        {
            // GET: /api/TipoUsuario
            app.MapGet("/api/TipoUsuario", GetTipoUsuarios);

            // GET: /api/TipoUsuario/{id}
            app.MapGet("/api/TipoUsuario/{id}", GetTipoUsuarioById);

            // POST: /api/TipoUsuario
            app.MapPost("/api/TipoUsuario", CreateTipoUsuario);

            // PUT: /api/TipoUsuario/{id}
            app.MapPut("/api/TipoUsuario/{id}", UpdateTipoUsuario);

            // DELETE: /api/TipoUsuario/{id}
            app.MapDelete("/api/TipoUsuario/{id}", DeleteTipoUsuario);

            // GET: /api/TipoUsuario/exists/{id}
            app.MapGet("/api/TipoUsuario/exists/{id}", ExistsTipoUsuario);

            // GET: /api/TipoUsuario/exists/nombre/{nombre}
            app.MapGet("/api/TipoUsuario/exists/nombre/{nombre}", ExistsTipoUsuarioByName);
        }

        // ========================================
        // HANDLERS
        // ========================================

        private static async Task<IResult> GetTipoUsuarios(ApplicationDbContext db)
        {
            var tipos = await db.TiposUsuario
                .Select(t => new TipoUsuarioDto
                {
                    Id = t.Id,
                    Nombre = t.Nombre
                })
                .ToListAsync();
            return Results.Ok(tipos);
        }

        private static async Task<IResult> GetTipoUsuarioById(int id, ApplicationDbContext db)
        {
            var tipo = await db.TiposUsuario.FindAsync(id);
            if (tipo == null)
                return Results.NotFound(new { message = "Tipo de usuario no encontrado" });

            return Results.Ok(new TipoUsuarioDto
            {
                Id = tipo.Id,
                Nombre = tipo.Nombre
            });
        }

        // ✅ CREATE - Con validación de duplicados
        private static async Task<IResult> CreateTipoUsuario(TipoUsuarioCreateDto dto, ApplicationDbContext db)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                return Results.BadRequest(new { error = "El nombre es requerido" });

            // ✅ Verificar si ya existe
            var exists = await db.TiposUsuario.AnyAsync(t => t.Nombre == dto.Nombre.Trim());
            if (exists)
                return Results.BadRequest(new { error = "Ya existe un tipo de usuario con ese nombre" });

            var tipo = new TipoUsuario
            {
                Nombre = dto.Nombre.Trim()
            };

            db.TiposUsuario.Add(tipo);
            await db.SaveChangesAsync();

            return Results.Created($"/api/TipoUsuario/{tipo.Id}", new TipoUsuarioDto
            {
                Id = tipo.Id,
                Nombre = tipo.Nombre
            });
        }

        private static async Task<IResult> UpdateTipoUsuario(int id, TipoUsuarioUpdateDto dto, ApplicationDbContext db)
        {
            var tipo = await db.TiposUsuario.FindAsync(id);
            if (tipo == null)
                return Results.NotFound(new { message = "Tipo de usuario no encontrado" });

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                return Results.BadRequest(new { error = "El nombre es requerido" });

            // ✅ Verificar si ya existe otro con el mismo nombre
            var exists = await db.TiposUsuario
                .AnyAsync(t => t.Nombre == dto.Nombre.Trim() && t.Id != id);
            if (exists)
                return Results.BadRequest(new { error = "Ya existe otro tipo de usuario con ese nombre" });

            tipo.Nombre = dto.Nombre.Trim();
            await db.SaveChangesAsync();

            return Results.Ok(new TipoUsuarioDto
            {
                Id = tipo.Id,
                Nombre = tipo.Nombre
            });
        }

        private static async Task<IResult> DeleteTipoUsuario(int id, ApplicationDbContext db)
        {
            var tipo = await db.TiposUsuario.FindAsync(id);
            if (tipo == null)
                return Results.NotFound(new { message = "Tipo de usuario no encontrado" });

            db.TiposUsuario.Remove(tipo);
            await db.SaveChangesAsync();

            return Results.NoContent();
        }

        private static async Task<IResult> ExistsTipoUsuario(int id, ApplicationDbContext db)
        {
            var exists = await db.TiposUsuario.AnyAsync(t => t.Id == id);
            return Results.Ok(new { exists });
        }

        private static async Task<IResult> ExistsTipoUsuarioByName(string nombre, int? excludeId, ApplicationDbContext db)
        {
            var query = db.TiposUsuario.Where(t => t.Nombre == nombre);
            if (excludeId.HasValue)
            {
                query = query.Where(t => t.Id != excludeId.Value);
            }
            var exists = await query.AnyAsync();
            return Results.Ok(new { exists });
        }
    }
}