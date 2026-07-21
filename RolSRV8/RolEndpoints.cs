using RolSRV8.Entities;
using RolSRV8.Services;

namespace RolSRV8;

public static class RolEndpoints
{
    public static void MapRolEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Rol")
            .WithTags("Rol");


        // GET /api/Rol
        group.MapGet("/", async (IRolService service) =>
        {
            var roles = await service.ObtenerTodosAsync();

            return Results.Ok(new
            {
                codigo = 200,
                mensaje = "OK",
                data = roles
            });
        });



        // GET /api/Rol/{id}
        group.MapGet("/{id}", async (
            int id,
            IRolService service) =>
        {
            var rol = await service.ObtenerPorIdAsync(id);

            if (rol == null)
            {
                return Results.NotFound(new
                {
                    codigo = 404,
                    mensaje = "Rol no encontrado"
                });
            }

            return Results.Ok(new
            {
                codigo = 200,
                mensaje = "OK",
                data = rol
            });
        });



        // POST /api/Rol
        group.MapPost("/", async (
            RolRequest request,
            IRolService service,
            IBitacoraClient bitacoraClient) =>
        {

            var resultado = await service.CrearAsync(request);


            if (!resultado.ok)
            {
                return Results.BadRequest(new
                {
                    codigo = 400,
                    mensaje = resultado.error
                });
            }

            var detalleJson = System.Text.Json.JsonSerializer.Serialize(new
            {
                Accion = "CREACION",
                Rol = new
                {
                    Nombre = request.Nombre,
                    Pantallas = request.Pantallas
                }
            });


            await bitacoraClient.RegistrarAsync(
                "Administrador del Sistema",
                $"Creó el rol {request.Nombre}",
                detalleJson);



            return Results.Created("/api/Rol", new
            {
                codigo = 201,
                mensaje = "Rol creado correctamente",
                data = new
                {
                    nombre = request.Nombre,
                    pantallas = request.Pantallas
                }
            });

        });



        // PUT /api/Rol/{id}
        group.MapPut("/{id}", async (
            int id,
            RolRequest request,
            IRolService service,
            IBitacoraClient bitacoraClient) =>
        {

            var rolAnterior = await service.ObtenerPorIdAsync(id);


            if (rolAnterior == null)
            {
                return Results.NotFound(new
                {
                    codigo = 404,
                    mensaje = "Rol no encontrado"
                });
            }


            var resultado = await service.ActualizarAsync(id, request);


            if (!resultado.ok)
            {
                return Results.BadRequest(new
                {
                    codigo = 400,
                    mensaje = resultado.error
                });
            }



            var detalleJson = System.Text.Json.JsonSerializer.Serialize(new
            {
                Accion = "ACTUALIZACION",

                Anterior = rolAnterior,

                Nuevo = new
                {
                    Nombre = request.Nombre,
                    Pantallas = request.Pantallas
                }
            });


            await bitacoraClient.RegistrarAsync(
                "Administrador del Sistema",
                $"Modificó el rol {request.Nombre}",
                detalleJson);



            return Results.Ok(new
            {
                codigo = 200,
                mensaje = "Rol actualizado correctamente",
                data = new
                {
                    id,
                    nombre = request.Nombre,
                    pantallas = request.Pantallas
                }
            });

        });



        // DELETE /api/Rol/{id}
        group.MapDelete("/{id}", async (
            int id,
            IRolService service,
            IBitacoraClient bitacoraClient) =>
        {

            var rol = await service.ObtenerPorIdAsync(id);


            if (rol == null)
            {
                return Results.NotFound(new
                {
                    codigo = 404,
                    mensaje = "Rol no encontrado"
                });
            }



            var resultado = await service.EliminarAsync(id);


            if (!resultado.ok)
            {
                return Results.BadRequest(new
                {
                    codigo = 400,
                    mensaje = resultado.error
                });
            }



            var detalleJson = System.Text.Json.JsonSerializer.Serialize(new
            {
                Accion = "ELIMINACION",
                Eliminado = rol
            });


            await bitacoraClient.RegistrarAsync(
                "Administrador del Sistema",
                $"Eliminó el rol {rol.Nombre}",
                detalleJson);



            return Results.Ok(new
            {
                codigo = 200,
                mensaje = "Rol eliminado correctamente"
            });

        });

    }
}