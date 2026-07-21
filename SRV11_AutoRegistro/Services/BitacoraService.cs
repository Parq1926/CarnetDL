using System.Net.Http.Json;

namespace SRV11_AutoRegistro.Services;

public class BitacoraService
{
    private readonly HttpClient _httpClient;
    private readonly string _bitacoraUrl;

    public BitacoraService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;

        _bitacoraUrl =
            configuration["Services:Bitacora"]
            ?? throw new InvalidOperationException(
                "La configuración Services:Bitacora no existe.");
    }

    public async Task Registrar(
        string usuario,
        string accion)
    {
        try
        {
            await _httpClient.PostAsJsonAsync(
                _bitacoraUrl,
                new
                {
                    Usuario = usuario,
                    Accion = accion,
                    Fecha = DateTime.Now
                });
        }
        catch
        {
        }
    }
}