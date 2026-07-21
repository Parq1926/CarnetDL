using System.Text.Json;

namespace SRV11_AutoRegistro.Services;

public interface IAreaService
{
    Task<AreaDto?> GetById(int id);
    Task<List<AreaDto>> GetAll();
}


public class AreaService : IAreaService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;


    public AreaService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }



    public async Task<AreaDto?> GetById(int id)
    {
        try
        {
            var areaUrl = _configuration["Services:Area"];


            var response = await _httpClient.GetAsync(
                $"{areaUrl}/areas/{id}");


            if (!response.IsSuccessStatusCode)
                return null;


            return await response.Content.ReadFromJsonAsync<AreaDto>();

        }
        catch
        {
            return null;
        }
    }



    public async Task<List<AreaDto>> GetAll()
    {
        try
        {
            var areaUrl = _configuration["Services:Area"];


            var response = await _httpClient.GetAsync(
                $"{areaUrl}/areas");


            if (!response.IsSuccessStatusCode)
                return new List<AreaDto>();


            var contenido = await response.Content.ReadAsStringAsync();


            using var document = JsonDocument.Parse(contenido);


            var lista = new List<AreaDto>();


            foreach (var item in document.RootElement.EnumerateArray())
            {
                lista.Add(new AreaDto
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
            Console.WriteLine($"Error AreaService.GetAll: {ex.Message}");
            return new List<AreaDto>();
        }
    }
}



public class AreaDto
{
    public int ID { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public int InstitucionID { get; set; }
}