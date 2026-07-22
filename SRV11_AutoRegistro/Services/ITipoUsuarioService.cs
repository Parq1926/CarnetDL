using System.Text.Json;

namespace SRV11_AutoRegistro.Services;

public interface ITipoUsuarioService
{
    Task<TipoUsuarioDto?> GetById(int id);
    Task<List<TipoUsuarioDto>> GetAll();
}


public class TipoUsuarioService : ITipoUsuarioService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public TipoUsuarioService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }


    public async Task<TipoUsuarioDto?> GetById(int id)
    {
        Console.WriteLine("========== GET TIPO USUARIO POR ID ==========");
        Console.WriteLine($"ID recibido: {id}");

        try
        {
            if (id <= 0)
            {
                Console.WriteLine("❌ ID inválido");
                return null;
            }


            var tipoUsuarioUrl =
                _configuration["Services:TipoUsuario"];


            if (string.IsNullOrWhiteSpace(tipoUsuarioUrl))
            {
                Console.WriteLine("❌ No existe configuración Services:TipoUsuario");
                return null;
            }


            Console.WriteLine(
                $"URL base TipoUsuario: {tipoUsuarioUrl}");


            var url =
                $"{tipoUsuarioUrl}/api/TipoUsuario";


            Console.WriteLine(
                $"URL llamada: {url}");


            var response =
                await _httpClient.GetAsync(url);


            Console.WriteLine(
                $"Status HTTP: {response.StatusCode}");


            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(
                    "❌ Error llamando al microservicio TipoUsuario");

                return null;
            }


            var contenido =
                await response.Content.ReadAsStringAsync();


            Console.WriteLine("Respuesta recibida:");
            Console.WriteLine(contenido);


            using var document =
                JsonDocument.Parse(contenido);



            if (!document.RootElement.TryGetProperty("data", out var data))
            {
                Console.WriteLine(
                    "❌ La respuesta no contiene propiedad data");

                return null;
            }


            if (data.ValueKind != JsonValueKind.Array)
            {
                Console.WriteLine(
                    "❌ La propiedad data no es un arreglo");

                return null;
            }



            foreach (var item in data.EnumerateArray())
            {
                if (!item.TryGetProperty("id", out var idProperty))
                    continue;


                var tipoId =
                    idProperty.GetInt32();


                if (tipoId == id)
                {
                    var nombre =
                        item.TryGetProperty("nombre", out var nombreProperty)
                        ? nombreProperty.GetString()
                        : "";


                    Console.WriteLine(
                        $"✅ Tipo encontrado: {tipoId} - {nombre}");


                    return new TipoUsuarioDto
                    {
                        ID = tipoId,
                        Nombre = nombre ?? ""
                    };
                }
            }


            Console.WriteLine(
                $"❌ No se encontró TipoUsuario con ID {id}");

            return null;

        }
        catch (JsonException ex)
        {
            Console.WriteLine(
                $"❌ Error procesando JSON TipoUsuario: {ex.Message}");

            return null;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(
                $"❌ Error HTTP TipoUsuario: {ex.Message}");

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"❌ Error general TipoUsuarioService: {ex.Message}");

            return null;
        }
    }



    public async Task<List<TipoUsuarioDto>> GetAll()
    {
        try
        {
            var tipoUsuarioUrl =
                _configuration["Services:TipoUsuario"];


            if (string.IsNullOrWhiteSpace(tipoUsuarioUrl))
            {
                Console.WriteLine(
                    "❌ No existe configuración Services:TipoUsuario");

                return new List<TipoUsuarioDto>();
            }


            var response =
                await _httpClient.GetAsync(
                    $"{tipoUsuarioUrl}/api/TipoUsuario");


            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(
                    $"❌ Error HTTP: {response.StatusCode}");

                return new List<TipoUsuarioDto>();
            }


            var contenido =
                await response.Content.ReadAsStringAsync();


            using var document =
                JsonDocument.Parse(contenido);


            if (!document.RootElement.TryGetProperty("data", out var data))
            {
                Console.WriteLine(
                    "❌ No existe data en respuesta");

                return new List<TipoUsuarioDto>();
            }


            var lista =
                new List<TipoUsuarioDto>();


            foreach (var item in data.EnumerateArray())
            {
                lista.Add(new TipoUsuarioDto
                {
                    ID = item.GetProperty("id").GetInt32(),

                    Nombre =
                        item.GetProperty("nombre")
                        .GetString() ?? ""
                });
            }


            Console.WriteLine(
                $"✅ Tipos cargados: {lista.Count}");

            return lista;

        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"❌ Error TipoUsuarioService.GetAll: {ex.Message}");

            return new List<TipoUsuarioDto>();
        }
    }
}



public class TipoUsuarioDto
{
    public int ID { get; set; }

    public string Nombre { get; set; } = string.Empty;
}