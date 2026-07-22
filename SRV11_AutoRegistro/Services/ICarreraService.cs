using System.Text.Json;

namespace SRV11_AutoRegistro.Services;

public interface ICarreraService
{
    Task<CarreraDto?> GetById(int id);
    Task<List<CarreraDto>> GetAll();
}

public class CarreraService : ICarreraService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public CarreraService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }


    public async Task<CarreraDto?> GetById(int id)
    {
        try
        {
            var carreraUrl = _configuration["Services:Carrera"];

            var response = await _httpClient.GetAsync(
                $"{carreraUrl}/carreras/{id}");

            if (!response.IsSuccessStatusCode)
                return null;


            return await response.Content.ReadFromJsonAsync<CarreraDto>();
        }
        catch
        {
            return null;
        }
    }


    public async Task<List<CarreraDto>> GetAll()
    {
        try
        {
            var carreraUrl = _configuration["Services:Carrera"];

            var response = await _httpClient.GetAsync(
                $"{carreraUrl}/carreras");


            if (!response.IsSuccessStatusCode)
                return new List<CarreraDto>();


            var contenido = await response.Content.ReadAsStringAsync();


            using var document = JsonDocument.Parse(contenido);


            var lista = new List<CarreraDto>();


            foreach (var item in document.RootElement.EnumerateArray())
            {
                lista.Add(new CarreraDto
                {
                    ID = item.GetProperty("id").GetInt32(),
                    Nombre = item.GetProperty("nombre").GetString() ?? "",
                    InstitucionID = item.GetProperty("institucionID").GetInt32()
                });
            }


            return lista;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error CarreraService.GetAll: {ex.Message}");
            return new List<CarreraDto>();
        }
    }
}


public class CarreraDto
{
    public int ID { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public int InstitucionID { get; set; }
}