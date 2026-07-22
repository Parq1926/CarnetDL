using BitacoraSRV9.Entities;
using BitacoraSRV9.Repository;

namespace BitacoraSRV9.Services;

public class BitacoraService : IBitacoraService
{
    private readonly BitacoraRepository _repository;

    public BitacoraService(BitacoraRepository repository)
    {
        _repository = repository;
    }

    public async Task<(bool ok, string error)> RegistrarAsync(BitacoraRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Usuario))
            return (false, "El usuario es requerido");

        if (string.IsNullOrWhiteSpace(request.Accion))
            return (false, "La acción es requerida");

        var bitacora = new Bitacora
        {
            Usuario = request.Usuario,
            Accion = request.Accion,
            DetalleJson = request.DetalleJson,
            EsError = request.EsError,
            Fecha = DateTime.Now
        };

        await _repository.GuardarAsync(bitacora);

        return (true, string.Empty);
    }

    public async Task<IEnumerable<Bitacora>> ObtenerTodosAsync()
    {
        return await _repository.ObtenerTodosAsync();
    }

    public async Task<BitacoraPaginadaResponse> ObtenerConFiltrosAsync(BitacoraFiltrosRequest filtros)
    {
        // Validar y corregir valores de paginación
        if (filtros.Pagina < 1) filtros.Pagina = 1;
        if (filtros.TamanoPagina < 1) filtros.TamanoPagina = 15;
        if (filtros.TamanoPagina > 100) filtros.TamanoPagina = 100;

        return await _repository.ObtenerConFiltrosAsync(filtros);
    }

    public async Task RegistrarErrorAsync(string usuario, string accion, string detalleJson)
    {
        var bitacora = new Bitacora
        {
            Usuario = usuario ?? "Sistema",
            Accion = $"ERROR: {accion}",
            DetalleJson = detalleJson,
            EsError = true,
            Fecha = DateTime.Now
        };

        await _repository.GuardarAsync(bitacora);
    }

    public async Task RegistrarErrorAsync(string usuario, Exception ex, string contexto)
    {
        var detalleJson = $"{{\"contexto\":\"{contexto}\",\"mensaje\":\"{ex.Message}\",\"stackTrace\":\"{ex.StackTrace?.Replace("\n", "\\n")}\"}}";

        var bitacora = new Bitacora
        {
            Usuario = usuario ?? "Sistema",
            Accion = $"ERROR: {contexto}",
            DetalleJson = detalleJson,
            EsError = true,
            Fecha = DateTime.Now
        };

        await _repository.GuardarAsync(bitacora);
    }
}