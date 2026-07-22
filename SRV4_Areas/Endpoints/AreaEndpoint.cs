using AreasSRV7.Services;
using SRV4_Areas.DTOs;
using SRV4_Areas.Services;

namespace SRV4_Areas;

public static class AreaEndpoint
{
    public static void MapAreaEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/Areas").WithTags("Areas");

        // GET: /api/Areas - Obtener todos
        group.MapGet("/", async (IAreaService service) =>
        {
            try
            {
                Console.WriteLine("📡 GET /api/Areas - Iniciando...");
                var areas = await service.GetAll();
                Console.WriteLine($"✅ GET /api/Areas - {areas.Count()} áreas encontradas");
                return Results.Ok(areas);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GET /api/Areas: {ex.Message}");
                Console.WriteLine($"❌ StackTrace: {ex.StackTrace}");
                return Results.Problem($"Error: {ex.Message}", statusCode: 500);
            }
        });

        // GET: /api/Areas/{id} - Obtener por ID
        group.MapGet("/{id}", async (int id, IAreaService service) =>
        {
            try
            {
                var area = await service.GetById(id);
                if (area == null)
                {
                    return Results.NotFound(new { error = $"Area con ID {id} no encontrada" });
                }
                return Results.Ok(area);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GET /api/Areas/{id}: {ex.Message}");
                return Results.Problem(ex.Message, statusCode: 500);
            }
        });

        // POST: /api/Areas - Crear
        group.MapPost("/", async (CreateAreaRequest request, IAreaService service) => // ✅ Eliminar IBitacoraService
        {
            try
            {
                var result = await service.Create(request);

                if (result.success)
                {
                    // ✅ Comentar bitácora
                    // await bitacora.Registrar("Administrador del Sistema", $"Creó el area {request.Nombre}");
                    var created = await service.GetById(result.id ?? 0);
                    return Results.Created($"/api/Areas/{result.id}", created);
                }

                return Results.BadRequest(new { error = result.message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en POST /api/Areas: {ex.Message}");
                return Results.Problem(ex.Message, statusCode: 500);
            }
        });

        // PUT: /api/Areas/{id} - Actualizar
        group.MapPut("/{id}", async (int id, UpdateAreaRequest request, IAreaService service) => // ✅ Eliminar IBitacoraService
        {
            try
            {
                if (id != request.ID)
                {
                    return Results.BadRequest(new { error = "El ID de la ruta no coincide con el ID del cuerpo" });
                }

                var result = await service.Update(request);

                if (result.success)
                {
                    // ✅ Comentar bitácora
                    // await bitacora.Registrar("Administrador del Sistema", $"Modificó el area {request.Nombre}");
                    var updated = await service.GetById(id);
                    return Results.Ok(updated);
                }

                return Results.BadRequest(new { error = result.message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en PUT /api/Areas/{id}: {ex.Message}");
                return Results.Problem(ex.Message, statusCode: 500);
            }
        });

        // DELETE: /api/Areas/{id} - Eliminar
        group.MapDelete("/{id}", async (int id, IAreaService service) => // ✅ Eliminar IBitacoraService
        {
            try
            {
                var area = await service.GetById(id);

                if (area == null)
                {
                    return Results.NotFound(new { error = $"Area con ID {id} no encontrada" });
                }

                var result = await service.Delete(id);

                if (result.success)
                {
                    // ✅ Comentar bitácora
                    // await bitacora.Registrar("Administrador del Sistema", $"Eliminó el area {area.Nombre}");
                    return Results.Ok(new { message = result.message });
                }

                return Results.BadRequest(new { error = result.message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en DELETE /api/Areas/{id}: {ex.Message}");
                return Results.Problem(ex.Message, statusCode: 500);
            }
        });
    }
}