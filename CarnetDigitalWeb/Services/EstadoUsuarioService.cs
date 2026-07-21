using System.Net.Http.Headers;
using System.Net.Http.Json;
using CarnetDigitalWeb.Models;

namespace CarnetDigitalWeb.Services
{
    public class EstadoUsuarioService : IEstadoUsuarioService
    {
        private readonly HttpClient _http;

        public EstadoUsuarioService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("EstadoUsuario");
        }

        public async Task<(bool Ok, string? Error, UsuarioEstado? Data)> CambiarEstadoAsync(
            int usuarioId, string estado, string token)
        {
            using var request = new HttpRequestMessage(HttpMethod.Patch, "usuarios/estado")
            {
                Content = JsonContent.Create(new { usuarioId, estado }, options: ApiHelper.JsonOpts)
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return (false, await ApiHelper.LeerErrorAsync(response), null);

            var data = await response.Content.ReadFromJsonAsync<UsuarioEstado>(ApiHelper.JsonOpts);
            return (true, null, data);
        }
    }
}
