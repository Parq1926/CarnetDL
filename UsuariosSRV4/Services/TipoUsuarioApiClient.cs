using System.Net;
using System.Net.Http.Json;
using UsuariosSRV4.DTOs;

namespace UsuariosSRV4.Services
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
    }
}