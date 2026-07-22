using System.Text;
using System.Text.Json;
using CarnetDigitalWeb.Models;

namespace CarnetDigitalWeb.Services
{
    public class TipoIdentificacionService : ITipoIdentificacionService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<TipoIdentificacionService> _logger;

        public TipoIdentificacionService(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<TipoIdentificacionService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<List<TipoIdentificacion>> GetAllAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("TipoIdentificacion");
                var token = GetToken();

                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var response = await client.GetAsync("/api/TipoIdentificacion");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<TipoIdentificacion>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<TipoIdentificacion>();
                }

                return new List<TipoIdentificacion>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetAllAsync");
                return new List<TipoIdentificacion>();
            }
        }

        public async Task<TipoIdentificacion?> GetByIdAsync(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("TipoIdentificacion");
                var token = GetToken();

                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var response = await client.GetAsync($"/api/TipoIdentificacion/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<TipoIdentificacion>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en GetByIdAsync({id})");
                return null;
            }
        }

        public async Task<bool> CreateAsync(TipoIdentificacionCreateDto dto)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("TipoIdentificacion");
                var token = GetToken();

                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var json = JsonSerializer.Serialize(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("/api/TipoIdentificacion", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en CreateAsync");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(TipoIdentificacionUpdateDto dto)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("TipoIdentificacion");
                var token = GetToken();

                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var json = JsonSerializer.Serialize(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"/api/TipoIdentificacion/{dto.Id}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en UpdateAsync");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("TipoIdentificacion");
                var token = GetToken();

                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var response = await client.DeleteAsync($"/api/TipoIdentificacion/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en DeleteAsync({id})");
                return false;
            }
        }

        private string? GetToken()
        {
            // ✅ Obtener token de la sesión o de los headers
            var context = _httpContextAccessor.HttpContext;
            if (context == null) return null;

            // Intentar obtener de la sesión
            var sessionToken = context.Session.GetString("Token");
            if (!string.IsNullOrEmpty(sessionToken))
            {
                return sessionToken;
            }

            // Intentar obtener del header Authorization
            if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                var auth = authHeader.ToString();
                if (auth.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    return auth.Substring("Bearer ".Length).Trim();
                }
            }

            return null;
        }
    }
}