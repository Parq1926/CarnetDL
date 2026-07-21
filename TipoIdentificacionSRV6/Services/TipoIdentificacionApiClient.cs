using System.Net;
using System.Net.Http.Json;
using TipoIdentificacionSRV6.DTOs;

namespace TipoIdentificacionSRV6.Services
{
    public class TipoIdentificacionApiClient : ITipoIdentificacionApiClient
    {
        private readonly HttpClient _http;

        public TipoIdentificacionApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<TipoIdentificacionDto>> GetAllAsync(CancellationToken ct = default)
        {
            var result = await _http.GetFromJsonAsync<List<TipoIdentificacionDto>>("api/TipoIdentificacion", ct);
            return result ?? new List<TipoIdentificacionDto>();
        }

        public async Task<TipoIdentificacionDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _http.GetFromJsonAsync<TipoIdentificacionDto>($"api/TipoIdentificacion/{id}", ct);
        }

        public async Task<(bool ok, HttpStatusCode status, string? message)> CreateAsync(TipoIdentificacionCreateDto dto, CancellationToken ct = default)
        {
            var response = await _http.PostAsJsonAsync("api/TipoIdentificacion", dto, ct);
            string? msg = null;
            try
            {
                var payload = await response.Content.ReadFromJsonAsync<Dictionary<string, string?>>(cancellationToken: ct);
                if (payload != null && payload.TryGetValue("message", out var m))
                {
                    msg = m;
                }
            }
            catch { /* ignorar parseos fallidos */ }
            return (response.IsSuccessStatusCode, response.StatusCode, msg);
        }

        public async Task<(bool ok, HttpStatusCode status, string? message)> UpdateAsync(int id, TipoIdentificacionUpdateDto dto, CancellationToken ct = default)
        {
            var response = await _http.PutAsJsonAsync($"api/TipoIdentificacion/{id}", dto, ct);
            string? msg = null;
            try
            {
                var payload = await response.Content.ReadFromJsonAsync<Dictionary<string, string?>>(cancellationToken: ct);
                if (payload != null && payload.TryGetValue("message", out var m))
                {
                    msg = m;
                }
            }
            catch { /* ignorar parseos fallidos */ }
            return (response.IsSuccessStatusCode, response.StatusCode, msg);
        }

        public async Task<(bool ok, HttpStatusCode status, string? message)> DeleteAsync(int id, CancellationToken ct = default)
        {
            var response = await _http.DeleteAsync($"api/TipoIdentificacion/{id}", ct);
            string? msg = null;
            try
            {
                var payload = await response.Content.ReadFromJsonAsync<Dictionary<string, string?>>(cancellationToken: ct);
                if (payload != null && payload.TryGetValue("message", out var m))
                {
                    msg = m;
                }
            }
            catch { /* ignorar parseos fallidos */ }
            return (response.IsSuccessStatusCode, response.StatusCode, msg);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        {
            var response = await _http.GetAsync($"api/TipoIdentificacion/{id}", ct);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ExistsByNameAsync(string nombre, int? excludeId = null, CancellationToken ct = default)
        {
            var url = $"api/TipoIdentificacion/exists?nombre={Uri.EscapeDataString(nombre)}";
            if (excludeId.HasValue)
            {
                url += $"&excludeId={excludeId.Value}";
            }
            var response = await _http.GetAsync(url, ct);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>(cancellationToken: ct);
                return result;
            }
            return false;
        }
    }
}