using System.Net;
using System.Net.Http.Json;
using TiposUsuarioSRV5.DTOs;

namespace TiposUsuarioSRV5.Services
{
    public class TipoUsuarioApiClient : ITipoUsuarioApiClient
    {
        private readonly HttpClient _http;

        public TipoUsuarioApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<TipoUsuarioDto>> GetAllAsync(CancellationToken ct = default)
        {
            var result = await _http.GetFromJsonAsync<List<TipoUsuarioDto>>("api/TipoUsuario", ct);
            return result ?? new List<TipoUsuarioDto>();
        }

        public async Task<TipoUsuarioDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _http.GetFromJsonAsync<TipoUsuarioDto>($"api/TipoUsuario/{id}", ct);
        }

        public async Task<(bool ok, HttpStatusCode status, string? message)> CreateAsync(TipoUsuarioCreateDto dto, CancellationToken ct = default)
        {
            var response = await _http.PostAsJsonAsync("api/TipoUsuario", dto, ct);
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

        public async Task<(bool ok, HttpStatusCode status, string? message)> UpdateAsync(int id, TipoUsuarioUpdateDto dto, CancellationToken ct = default)
        {
            var response = await _http.PutAsJsonAsync($"api/TipoUsuario/{id}", dto, ct);
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
            var response = await _http.DeleteAsync($"api/TipoUsuario/{id}", ct);
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
            var response = await _http.GetAsync($"api/TipoUsuario/exists/{id}", ct);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ExistsByNameAsync(string nombre, int? excludeId = null, CancellationToken ct = default)
        {
            var url = $"api/TipoUsuario/exists/nombre/{Uri.EscapeDataString(nombre)}";
            if (excludeId.HasValue)
            {
                url += $"?excludeId={excludeId.Value}";
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