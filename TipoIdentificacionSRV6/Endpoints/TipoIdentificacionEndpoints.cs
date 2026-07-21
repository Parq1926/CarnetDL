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
            var group = app.MapGroup("/api/TipoIdentificacion");

            // GET: api/TipoIdentificacion
            group.MapGet("/", async (ApplicationDbContext db) =>
            {
                var tipos = await db.TiposIdentificacion
                    .Select(t => new TipoIdentificacionDto
                    {
                        Id = t.Id,
                        Nombre = t.Nombre
                    })
                    .ToListAsync();
                return Results.Ok(tipos);
            });

            // GET: api/TipoIdentificacion/{id}
            group.MapGet("/{id}", async (int id, ApplicationDbContext db) =>
            {
                var tipo = await db.TiposIdentificacion.FindAsync(id);
                if (tipo == null)
                    return Results.NotFound();

                return Results.Ok(new TipoIdentificacionDto
                {
                    Id = tipo.Id,
                    Nombre = tipo.Nombre
                });
            });

            // POST: api/TipoIdentificacion
            group.MapPost("/", async (TipoIdentificacionCreateDto dto, ApplicationDbContext db) =>
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
            });

            // PUT: api/TipoIdentificacion/{id}
            group.MapPut("/{id}", async (int id, TipoIdentificacionUpdateDto dto, ApplicationDbContext db) =>
            {
                var tipo = await db.TiposIdentificacion.FindAsync(id);
                if (tipo == null)
                    return Results.NotFound();

                if (string.IsNullOrWhiteSpace(dto.Nombre))
                    return Results.BadRequest(new { error = "El nombre es requerido" });

                tipo.Nombre = dto.Nombre.Trim();
                await db.SaveChangesAsync();

                return Results.Ok(new TipoIdentificacionDto
                {
                    Id = tipo.Id,
                    Nombre = tipo.Nombre
                });
            });

            // DELETE: api/TipoIdentificacion/{id}
            group.MapDelete("/{id}", async (int id, ApplicationDbContext db) =>
            {
                var tipo = await db.TiposIdentificacion.FindAsync(id);
                if (tipo == null)
                    return Results.NotFound();

                db.TiposIdentificacion.Remove(tipo);
                await db.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}