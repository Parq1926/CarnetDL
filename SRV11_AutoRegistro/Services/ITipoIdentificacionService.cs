using System.Text.Json;

namespace SRV11_AutoRegistro.Services;

public interface ITipoIdentificacionService
{
    Task<TipoIdentificacionDto?> GetById(int id);
    Task<List<TipoIdentificacionDto>> GetAll();
}

public class TipoIdentificacionService : ITipoIdentificacionService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;


    public TipoIdentificacionService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }


    public async Task<TipoIdentificacionDto?> GetById(int id)
    {
        Console.WriteLine("========== GET TIPO IDENTIFICACION POR ID ==========");
        Console.WriteLine($"ID recibido: {id}");

        try
        {
            if (id <= 0)
            {
                Console.WriteLine("❌ ID inválido");
                return null;
            }

            var tipoIdentificacionUrl =
                _configuration["Services:TipoIdentificacion"];

            if (string.IsNullOrWhiteSpace(tipoIdentificacionUrl))
            {
                Console.WriteLine("❌ No existe configuración Services:TipoIdentificacion");
                return null;
            }

            var url = $"{tipoIdentificacionUrl}/api/TipoIdentificacion";

            Console.WriteLine($"URL llamada: {url}");

            var response = await _httpClient.GetAsync(url);

            Console.WriteLine($"Status HTTP: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
                return null;

            var contenido = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Respuesta recibida:");
            Console.WriteLine(contenido);

            using var document = JsonDocument.Parse(contenido);

            if (!document.RootElement.TryGetProperty("data", out var data))
            {
                Console.WriteLine("❌ No existe la propiedad data");
                return null;
            }

            foreach (var item in data.EnumerateArray())
            {
                var tipoId = item.GetProperty("id").GetInt32();

                if (tipoId == id)
                {
                    var nombre = item.GetProperty("nombre").GetString() ?? "";

                    Console.WriteLine($"✅ Tipo encontrado: {tipoId} - {nombre}");

                    return new TipoIdentificacionDto
                    {
                        ID = tipoId,
                        Nombre = nombre
                    };
                }
            }

            Console.WriteLine($"❌ No se encontró el tipo de identificación {id}");

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error TipoIdentificacionService: {ex.Message}");
            return null;
        }
    }



    public async Task<List<TipoIdentificacionDto>> GetAll()
    {
        try
        {
            var tipoIdentificacionUrl =
                _configuration["Services:TipoIdentificacion"];


            var response = await _httpClient.GetAsync(
                $"{tipoIdentificacionUrl}/api/TipoIdentificacion");


            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return new List<TipoIdentificacionDto>();
            }


            var contenido =
                await response.Content.ReadAsStringAsync();


            using var document =
                JsonDocument.Parse(contenido);


            var lista =
                new List<TipoIdentificacionDto>();


            foreach (var item in document.RootElement.GetProperty("data").EnumerateArray())
            {
                lista.Add(new TipoIdentificacionDto
                {
                    ID = item.GetProperty("id").GetInt32(),
                    Nombre = item.GetProperty("nombre").GetString() ?? ""
                });
            }


            return lista;

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error TipoIdentificacionService.GetAll: {ex}");
            return new List<TipoIdentificacionDto>();
        }
    }
}



public class TipoIdentificacionDto
{
    public int ID { get; set; }

    public string Nombre { get; set; } = string.Empty;
}