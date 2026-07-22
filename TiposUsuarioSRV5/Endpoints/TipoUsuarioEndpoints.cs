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
            var group = app.MapGroup("/api/TipoUsuario");

            // GET: api/TipoUsuario
            group.MapGet("/", async (ApplicationDbContext db) =>
            {
                var tipos = await db.TiposUsuario
                    .Select(t => new TipoUsuarioDto
                    {
                        Id = t.Id,
                        Nombre = t.Nombre
                    })
                    .ToListAsync();
                return Results.Ok(tipos);
            });



            // GET: api/TipoUsuario/{id}
            group.MapGet("/{id}", async (int id, ApplicationDbContext db) =>
            {
                var tipo = await db.TiposUsuario.FindAsync(id);
                if (tipo == null)
                    return Results.NotFound();

                return Results.Ok(new TipoUsuarioDto
                {
                    Id = tipo.Id,
                    Nombre = tipo.Nombre
                });
            });

            // POST: api/TipoUsuario
            group.MapPost("/", async (TipoUsuarioCreateDto dto, ApplicationDbContext db) =>
            {
                if (string.IsNullOrWhiteSpace(dto.Nombre))
                    return Results.BadRequest(new { error = "El nombre es requerido" });

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
            });

            // PUT: api/TipoUsuario/{id}
            group.MapPut("/{id}", async (int id, TipoUsuarioUpdateDto dto, ApplicationDbContext db) =>
            {
                var tipo = await db.TiposUsuario.FindAsync(id);
                if (tipo == null)
                    return Results.NotFound();

                if (string.IsNullOrWhiteSpace(dto.Nombre))
                    return Results.BadRequest(new { error = "El nombre es requerido" });

                tipo.Nombre = dto.Nombre.Trim();
                await db.SaveChangesAsync();

                return Results.Ok(new TipoUsuarioDto
                {
                    Id = tipo.Id,
                    Nombre = tipo.Nombre
                });
            });

            // DELETE: api/TipoUsuario/{id}
            group.MapDelete("/{id}", async (int id, ApplicationDbContext db) =>
            {
                var tipo = await db.TiposUsuario.FindAsync(id);
                if (tipo == null)
                    return Results.NotFound();

                db.TiposUsuario.Remove(tipo);
                await db.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}