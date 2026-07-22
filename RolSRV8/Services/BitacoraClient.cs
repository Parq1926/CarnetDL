using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RolSRV8.Services;

public class BitacoraClient : IBitacoraClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public BitacoraClient(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task RegistrarAsync(
        string usuario,
        string accion,
        string detalleJson)
    {
        var bitacoraUrl = _configuration["Services:Bitacora"];

        await _httpClient.PostAsJsonAsync(
            bitacoraUrl,
            new
            {
                Usuario = usuario,
                Accion = accion,
                DetalleJson = detalleJson,
                EsError = false
            });
    }
}