using System.Text.Json;

namespace SRV11_AutoRegistro.Services;


public interface IInstitucionService
{
    Task<InstitucionDto?> GetById(int id);
    Task<List<InstitucionDto>> GetAll();
}



public class InstitucionService : IInstitucionService
{

    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;


    public InstitucionService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }



    public async Task<InstitucionDto?> GetById(int id)
    {
        try
        {
            var institucionUrl = _configuration["Services:Institucion"];


            var response = await _httpClient.GetAsync(
                $"{institucionUrl}/institucion/{id}");


            if (!response.IsSuccessStatusCode)
                return null;


            return await response.Content.ReadFromJsonAsync<InstitucionDto>();

        }
        catch
        {
            return null;
        }
    }



    public async Task<List<InstitucionDto>> GetAll()
    {

        try
        {

            var institucionUrl =
                _configuration["Services:Institucion"];


            var response = await _httpClient.GetAsync(
                $"{institucionUrl}/institucion");


            if (!response.IsSuccessStatusCode)
                return new List<InstitucionDto>();


            var contenido =
                await response.Content.ReadAsStringAsync();


            using var document =
                JsonDocument.Parse(contenido);


            var lista =
                new List<InstitucionDto>();


            foreach (var item in document.RootElement.EnumerateArray())
            {

                lista.Add(new InstitucionDto
                {
                    ID = item.GetProperty("id").GetInt32(),
                    Nombre = item.GetProperty("nombre").GetString() ?? "",
                    Dominios = item.GetProperty("dominios").GetString() ?? ""
                });

            }


            return lista;

        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Error InstitucionService.GetAll: {ex.Message}");

            return new List<InstitucionDto>();
        }

    }

}



public class InstitucionDto
{
    public int ID { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Dominios { get; set; } = string.Empty;
}