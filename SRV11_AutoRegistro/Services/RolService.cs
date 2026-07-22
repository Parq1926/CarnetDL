using System.Text.Json;
using SRV11_AutoRegistro.Entities;

namespace SRV11_AutoRegistro.Services
{
    public class RolService : IRolService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;


        public RolService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }


        public async Task<Rol?> GetById(int id)
        {
            try
            {
                var url =
                    _configuration["Services:Rol"];


                var response =
                    await _httpClient.GetAsync(
                        $"{url}/api/rol/{id}");


                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(
                        $"Error consultando rol: {response.StatusCode}");

                    return null;
                }


                var contenido =
                    await response.Content.ReadAsStringAsync();


                Console.WriteLine("Respuesta Rol:");
                Console.WriteLine(contenido);


                return JsonSerializer.Deserialize<Rol>(
                    contenido,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error RolService.GetById: {ex.Message}");

                return null;
            }
        }



        public async Task<List<Rol>> GetAll()
        {
            try
            {
                var url =
                    _configuration["Services:Rol"];


                var response =
                    await _httpClient.GetAsync(
                        $"{url}/api/rol");


                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(
                        $"Error consultando roles: {response.StatusCode}");

                    return new List<Rol>();
                }


                var contenido =
                    await response.Content.ReadAsStringAsync();


                Console.WriteLine("Respuesta Roles:");
                Console.WriteLine(contenido);



                return JsonSerializer.Deserialize<List<Rol>>(
                    contenido,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    })
                    ?? new List<Rol>();

            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error RolService.GetAll: {ex.Message}");

                return new List<Rol>();
            }
        }
    }
}