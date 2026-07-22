using System.Text;
using System.Text.Json;
using CarnetDigitalWeb.Models;

namespace CarnetDigitalWeb.Services
{
    public class TipoUsuarioService : ITipoUsuarioService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<TipoUsuarioService> _logger;

        public TipoUsuarioService(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<TipoUsuarioService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<List<TipoUsuario>> GetAllAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("TipoUsuario");
                var token = GetToken();

                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var response = await client.GetAsync("/api/TipoUsuario");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<TipoUsuario>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<TipoUsuario>();
                }

                return new List<TipoUsuario>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetAllAsync");
                return new List<TipoUsuario>();
            }
        }

        public async Task<TipoUsuario?> GetByIdAsync(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("TipoUsuario");
                var token = GetToken();

                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var response = await client.GetAsync($"/api/TipoUsuario/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<TipoUsuario>(json, new JsonSerializerOptions
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

        public async Task<bool> CreateAsync(TipoUsuarioCreateDto dto)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("TipoUsuario");
                var token = GetToken();

                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var json = JsonSerializer.Serialize(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("/api/TipoUsuario", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en CreateAsync");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(TipoUsuarioUpdateDto dto)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("TipoUsuario");
                var token = GetToken();

                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var json = JsonSerializer.Serialize(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"/api/TipoUsuario/{dto.Id}", content);
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
                var client = _httpClientFactory.CreateClient("TipoUsuario");
                var token = GetToken();

                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var response = await client.DeleteAsync($"/api/TipoUsuario/{id}");
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
            var context = _httpContextAccessor.HttpContext;
            if (context == null) return null;

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