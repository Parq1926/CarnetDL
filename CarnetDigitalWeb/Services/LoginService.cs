using System.Text;
using System.Text.Json;
using CarnetDigitalWeb.Models;

namespace CarnetDigitalWeb.Services
{
    public class LoginService : ILoginService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<LoginService> _logger;

        public LoginService(IHttpClientFactory httpClientFactory, ILogger<LoginService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Login");

                var loginData = new
                {
                    email = request.Email,
                    password = request.Password,
                    tipo = request.Tipo ?? ""
                };

                var json = JsonSerializer.Serialize(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("api/auth/login", content);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Login fallido: {StatusCode}", response.StatusCode);
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Credenciales inválidas"
                    };
                }

                var responseJson = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<LoginResponse>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result ?? new LoginResponse
                {
                    Success = false,
                    Message = "Error al procesar respuesta"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en LoginAsync");
                return new LoginResponse
                {
                    Success = false,
                    Message = $"Error de conexión: {ex.Message}"
                };
            }
        }
    }
}