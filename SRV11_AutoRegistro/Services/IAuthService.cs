using System.Net.Http.Json;
using System.Text.Json;

namespace SRV11_AutoRegistro.Services;

public interface IAuthService
{
    Task<string?> ObtenerTokenAsync();
}


public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;


    public AuthService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }



    public async Task<string?> ObtenerTokenAsync()
    {

        var loginUrl =
            $"{_configuration["Services:Login"]}/api/Auth/login";



        var login = new
        {
            usuario = "servicio@autoregistro.cr",
            contrasena = "123456",
            tipo = "Estudiante"
        };



        var response = await _httpClient.PostAsJsonAsync(
            loginUrl,
            login);



        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine(
                $"Error login: {response.StatusCode}");

            return null;
        }



        var json = await response.Content.ReadAsStringAsync();



        using var document = JsonDocument.Parse(json);



        return document.RootElement
            .GetProperty("access_token")
            .GetString();

    }
}