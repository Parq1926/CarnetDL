using System.Net;
using System.Net.Http.Json;
using UsuariosSRV4.DTOs;

namespace UsuariosSRV4.Services
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
    }
}