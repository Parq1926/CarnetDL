using SRV3_Carreras.Services;

namespace SRV3_Carreras;

public static class CarreraEndpoint
{
    public static void MapCarreraEndpoints(this WebApplication app)
    {
        // ✅ Cambiar a /api/carreras para evitar conflicto
        var group = app.MapGroup("/api/carreras").WithTags("Carreras");

        group.MapGet("/", async (ICarreraService service) =>
        {
            var carreras = await service.GetAll();
            return Results.Ok(carreras);
        });

        group.MapGet("/{id}", async (int id, ICarreraService service) =>
        {
            var carrera = await service.GetById(id);
            if (carrera != null)
            {
                return Results.Ok(carrera);
            }
            return Results.NotFound($"Carrera con ID {id} no encontrada");
        });

        group.MapPost("/", async (CreateCarreraRequest request, ICarreraService service, IBitacoraService bitacora) =>
        {
            var result = await service.Create(request);

            if (result.success)
            {
                await bitacora.Registrar("Administrador del Sistema", $"Creó la carrera {request.Nombre}");

                return Results.Created($"/api/carreras/{result.id}", new
                {
                    message = result.message,
                    carrera = new
                    {
                        id = result.id,
                        nombre = request.Nombre,
                        director = request.Director,
                        email = request.Email,
                        telefono = request.Telefono,
                        institucionID = request.InstitucionID,
                        institucionNombre = request.InstitucionNombre
                    }
                });
            }

            return Results.BadRequest(new { error = result.message });
        });

        group.MapPut("/{id}", async (int id, UpdateCarreraRequest request, ICarreraService service, IBitacoraService bitacora) =>
        {
            if (id != request.ID)
            {
                return Results.BadRequest(new { error = "El ID de la ruta no coincide con el ID del cuerpo" });
            }

            var result = await service.Update(request);

            if (result.success)
            {
                await bitacora.Registrar("Administrador del Sistema", $"Modificó la carrera {request.Nombre}");

                return Results.Ok(new
                {
                    message = result.message,
                    carrera = new
                    {
                        id = request.ID,
                        nombre = request.Nombre,
                        director = request.Director,
                        email = request.Email,
                        telefono = request.Telefono,
                        institucionID = request.InstitucionID,
                        institucionNombre = request.InstitucionNombre
                    }
                });
            }

            return Results.BadRequest(new { error = result.message });
        });

        group.MapDelete("/{id}", async (int id, ICarreraService service, IBitacoraService bitacora) =>
        {
            var carrera = await service.GetById(id);

            if (carrera == null)
            {
                return Results.NotFound(new
                {
                    error = $"Carrera con ID {id} no encontrada"
                });
            }

            var result = await service.Delete(id);

            if (result.success)
            {
                await bitacora.Registrar("Administrador del Sistema", $"Eliminó la carrera {carrera.Nombre}");

                return Results.Ok(new
                {
                    message = result.message,
                    carrera = new
                    {
                        id = carrera.ID,
                        nombre = carrera.Nombre,
                        director = carrera.Director,
                        email = carrera.Email,
                        telefono = carrera.Telefono,
                        institucionID = carrera.InstitucionID,
                        institucionNombre = carrera.InstitucionNombre
                    }
                });
            }

            return Results.BadRequest(new { error = result.message });
        });
    }
}