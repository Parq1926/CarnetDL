using System.Net;
using System.Net.Http.Json;
using UsuariosSRV4.DTOs;

namespace UsuariosSRV4.Services
{
    public class CarreraApiClient : ICarreraApiClient
    {
        private readonly HttpClient _http;

        public CarreraApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<CarreraDto>> GetAllAsync(CancellationToken ct = default)
        {
            var result = await _http.GetFromJsonAsync<List<CarreraDto>>("carreras", ct);
            return result ?? new List<CarreraDto>();
        }

        public async Task<CarreraDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _http.GetFromJsonAsync<CarreraDto>($"carreras/{id}", ct);
        }

        public async Task<(bool ok, HttpStatusCode status, string? message)> CreateAsync(CarreraCreateDto dto, CancellationToken ct = default)
        {
            var response = await _http.PostAsJsonAsync("carreras", dto, ct);
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

        public async Task<(bool ok, HttpStatusCode status, string? message)> UpdateAsync(int id, CarreraUpdateDto dto, CancellationToken ct = default)
        {
            var response = await _http.PutAsJsonAsync($"carreras/{id}", dto, ct);
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
            var response = await _http.DeleteAsync($"carreras/{id}", ct);
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