using BitacoraSRV9.Entities;
using BitacoraSRV9.Services;
using BitacoraSRV9.Helpers;

namespace BitacoraSRV9;

public static class BitacoraEndpoints
{
    public static void MapBitacoraEndpoints(
        this IEndpointRouteBuilder routes)
    {
        // POST - Registrar en bitacora
        routes.MapPost("/bitacora",
        async (
            BitacoraRequest request,
            IBitacoraService service) =>
        {
            try
            {
                var resultado =
                    await service.RegistrarAsync(request);

                if (!resultado.ok)
                {
                    await service.RegistrarErrorAsync(
                        request.Usuario,
                        "Error al registrar en bitácora",
                        BitacoraHelper.CrearJsonError(resultado.error)
                    );

                    return Results.BadRequest(new
                    {
                        mensaje = resultado.error,
                        usuario = request.Usuario,
                        accion = request.Accion
                    });
                }

                return Results.Ok(new
                {
                    mensaje = "Movimiento registrado correctamente",
                    usuario = request.Usuario,
                    accion = request.Accion,
                    fecha = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                await service.RegistrarErrorAsync(
                    "Sistema",
                    ex,
                    "POST /bitacora"
                );

                return Results.BadRequest(new
                {
                    mensaje = "Error al registrar en bitacora",
                    error = ex.Message
                });
            }
        });

        // GET - Obtener bitácoras con filtros y paginación (NUEVO)
        routes.MapGet("/bitacora/filtros",
        async (
            [AsParameters] BitacoraFiltrosRequest filtros,
            IBitacoraService service) =>
        {
            try
            {
                var resultado =
                    await service.ObtenerConFiltrosAsync(filtros);

                return Results.Ok(resultado);
            }
            catch (Exception ex)
            {
                await service.RegistrarErrorAsync(
                    "Sistema",
                    ex,
                    "GET /bitacora/filtros"
                );

                return Results.BadRequest(new
                {
                    mensaje = "Error al obtener bitacora con filtros",
                    error = ex.Message
                });
            }
        });

        // GET - Obtener todos (mantener para compatibilidad)
        routes.MapGet("/bitacora",
        async (
            IBitacoraService service) =>
        {
            try
            {
                var lista =
                    await service.ObtenerTodosAsync();

                return Results.Ok(lista);
            }
            catch (Exception ex)
            {
                await service.RegistrarErrorAsync(
                    "Sistema",
                    ex,
                    "GET /bitacora"
                );

                return Results.BadRequest(new
                {
                    mensaje = "Error al obtener bitacora",
                    error = ex.Message
                });
            }
        });
    }
}