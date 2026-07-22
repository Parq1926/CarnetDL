using System.Net.Http.Headers;
using System.Net.Http.Json;
using CarnetDigitalWeb.Models;

namespace CarnetDigitalWeb.Services
{
    public class FotografiaService : IFotografiaService
    {
        private readonly HttpClient _http;

        public FotografiaService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("Fotografia");
        }

        public async Task<(bool Ok, string? Error, string? FotoBase64)> ObtenerAsync(int usuarioId, string token)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"usuario/fotografia/{usuarioId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return (false, await ApiHelper.LeerErrorAsync(response), null);

            // SRV13 GET devuelve el string base64 directo, no un objeto
            var foto = await response.Content.ReadFromJsonAsync<string>(ApiHelper.JsonOpts);
            return (true, null, foto);
        }

        public async Task<(bool Ok, string? Error, FotografiaUsuario? Data)> ActualizarAsync(
            int usuarioId, string fotoBase64, string token)
        {
            using var request = new HttpRequestMessage(HttpMethod.Put, "usuario/fotografia")
            {
                Content = JsonContent.Create(
                    new { usuarioId, fotografiaBase64 = fotoBase64 }, options: ApiHelper.JsonOpts)
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return (false, await ApiHelper.LeerErrorAsync(response), null);

            var data = await response.Content.ReadFromJsonAsync<FotografiaUsuario>(ApiHelper.JsonOpts);
            return (true, null, data);
        }

        public async Task<(bool Ok, string? Error)> EliminarAsync(int usuarioId, string token)
        {
            using var request = new HttpRequestMessage(HttpMethod.Delete, $"usuario/fotografia/{usuarioId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return (false, await ApiHelper.LeerErrorAsync(response));

            return (true, null);
        }
    }
}
