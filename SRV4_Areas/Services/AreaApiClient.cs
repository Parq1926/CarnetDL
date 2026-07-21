using System.Net;
using System.Net.Http.Json;
using AreasSRV7.DTOs;

namespace AreasSRV7.Services
{
    public class AreaApiClient : IAreaApiClient
    {
        private readonly HttpClient _http;

        public AreaApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<AreaDto>> GetAllAsync(CancellationToken ct = default)
        {
            var result = await _http.GetFromJsonAsync<List<AreaDto>>("api/Areas", ct);
            return result ?? new List<AreaDto>();
        }

        public async Task<AreaDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _http.GetFromJsonAsync<AreaDto>($"api/Areas/{id}", ct);
        }

        public async Task<(bool ok, HttpStatusCode status, string? message, AreaDto? data)> CreateAsync(AreaCreateDto dto, CancellationToken ct = default)
        {
            var response = await _http.PostAsJsonAsync("api/Areas", dto, ct);
            string? msg = null;
            AreaDto? data = null;
            try
            {
                var payload = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>(cancellationToken: ct);
                if (payload != null)
                {
                    if (payload.TryGetValue("message", out var m)) msg = m?.ToString();
                    if (payload.TryGetValue("area", out var d))
                    {
                        data = System.Text.Json.JsonSerializer.Deserialize<AreaDto>(d.ToString() ?? "{}");
                    }
                }
            }
            catch { /* ignorar parseos fallidos */ }
            return (response.IsSuccessStatusCode, response.StatusCode, msg, data);
        }

        public async Task<(bool ok, HttpStatusCode status, string? message, AreaDto? data)> UpdateAsync(int id, AreaUpdateDto dto, CancellationToken ct = default)
        {
            var response = await _http.PutAsJsonAsync($"api/Areas/{id}", dto, ct);
            string? msg = null;
            AreaDto? data = null;
            try
            {
                var payload = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>(cancellationToken: ct);
                if (payload != null)
                {
                    if (payload.TryGetValue("message", out var m)) msg = m?.ToString();
                    if (payload.TryGetValue("area", out var d))
                    {
                        data = System.Text.Json.JsonSerializer.Deserialize<AreaDto>(d.ToString() ?? "{}");
                    }
                }
            }
            catch { /* ignorar parseos fallidos */ }
            return (response.IsSuccessStatusCode, response.StatusCode, msg, data);
        }

        public async Task<(bool ok, HttpStatusCode status, string? message)> DeleteAsync(int id, CancellationToken ct = default)
        {
            var response = await _http.DeleteAsync($"api/Areas/{id}", ct);
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
    }
}