using BitacoraSRV9.Entities;

namespace BitacoraSRV9.Services;

public interface IBitacoraService
{
    Task<(bool ok, string error)> RegistrarAsync(BitacoraRequest request);
    Task<IEnumerable<Bitacora>> ObtenerTodosAsync();

    // Nuevo método para obtener con filtros y paginación
    Task<BitacoraPaginadaResponse> ObtenerConFiltrosAsync(BitacoraFiltrosRequest filtros);

    // Métodos para registrar errores
    Task RegistrarErrorAsync(string usuario, string accion, string detalleJson);
    Task RegistrarErrorAsync(string usuario, Exception ex, string contexto);
}