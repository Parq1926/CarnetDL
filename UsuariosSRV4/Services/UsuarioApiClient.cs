using System.Net;
using System.Net.Http.Json;
using UsuariosSRV4.DTOs;

namespace UsuariosSRV4.Services
{
    public class UsuarioApiClient : IUsuarioApiClient
    {
        private readonly HttpClient _http;

        public UsuarioApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<UsuarioDto>> GetAllAsync(CancellationToken ct = default)
        {
            var result = await _http.GetFromJsonAsync<List<UsuarioDto>>("api/Usuarios", ct);
            return result ?? new List<UsuarioDto>();
        }

        public async Task<UsuarioDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _http.GetFromJsonAsync<UsuarioDto>($"api/Usuarios/{id}", ct);
        }

        public async Task<(bool ok, HttpStatusCode status, string? message, UsuarioDto? data)> CreateAsync(CrearUsuarioDto dto, CancellationToken ct = default)
        {
            var response = await _http.PostAsJsonAsync("api/Usuarios", dto, ct);
            string? msg = null;
            UsuarioDto? data = null;
            try
            {
                var payload = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>(cancellationToken: ct);
                if (payload != null)
                {
                    if (payload.TryGetValue("message", out var m)) msg = m?.ToString();
                    if (payload.TryGetValue("data", out var d))
                    {
                        data = System.Text.Json.JsonSerializer.Deserialize<UsuarioDto>(d.ToString() ?? "{}");
                    }
                }
            }
            catch { /* ignorar parseos fallidos */ }
            return (response.IsSuccessStatusCode, response.StatusCode, msg, data);
        }

        public async Task<(bool ok, HttpStatusCode status, string? message, UsuarioDto? data)> UpdateAsync(int id, ActualizarUsuarioDto dto, CancellationToken ct = default)
        {
            var response = await _http.PutAsJsonAsync($"api/Usuarios/{id}", dto, ct);
            string? msg = null;
            UsuarioDto? data = null;
            try
            {
                var payload = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>(cancellationToken: ct);
                if (payload != null)
                {
                    if (payload.TryGetValue("message", out var m)) msg = m?.ToString();
                    if (payload.TryGetValue("data", out var d))
                    {
                        data = System.Text.Json.JsonSerializer.Deserialize<UsuarioDto>(d.ToString() ?? "{}");
                    }
                }
            }
            catch { /* ignorar parseos fallidos */ }
            return (response.IsSuccessStatusCode, response.StatusCode, msg, data);
        }

        public async Task<(bool ok, HttpStatusCode status, string? message)> DeleteAsync(int id, CancellationToken ct = default)
        {
            var response = await _http.DeleteAsync($"api/Usuarios/{id}", ct);
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

        public async Task<bool> DesbloquearUsuarioAsync(int id, CancellationToken ct = default)
        {
            var response = await _http.PutAsync($"api/Usuarios/{id}/desbloquear", null, ct);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<UsuarioDto>> GetByFilterAsync(FiltroUsuarioDto filtro, CancellationToken ct = default)
        {
            var response = await _http.PostAsJsonAsync("api/Usuarios/buscar", filtro, ct);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<UsuarioDto>>(cancellationToken: ct);
                return result ?? new List<UsuarioDto>();
            }
            return new List<UsuarioDto>();
        }

        public async Task<ValidarCredencialesResponse?> ValidarCredencialesAsync(string email, string password, string tipo, CancellationToken ct = default)
        {
            var request = new { email, password, tipo };
            var response = await _http.PostAsJsonAsync("api/Usuarios/validar-credenciales", request, ct);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ValidarCredencialesResponse>(cancellationToken: ct);
            }
            return null;
        }
    }
}