using System.Net.Http.Headers;
using System.Net.Http.Json;
using CarnetDigitalWeb.Models;

namespace CarnetDigitalWeb.Services
{
    public class ParametroService : IParametroService
    {
        private readonly HttpClient _http;

        public ParametroService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("Parametro");
        }

        private static HttpRequestMessage NuevoRequest(HttpMethod metodo, string ruta, string token, object? body = null)
        {
            var request = new HttpRequestMessage(metodo, ruta);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            if (body is not null)
                request.Content = JsonContent.Create(body, options: ApiHelper.JsonOpts);
            return request;
        }

        public async Task<(bool Ok, string? Error, List<Parametro>? Data)> GetAllAsync(string token)
        {
            using var request = NuevoRequest(HttpMethod.Get, "parametro", token);
            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return (false, await ApiHelper.LeerErrorAsync(response), null);

            var data = await response.Content.ReadFromJsonAsync<List<Parametro>>(ApiHelper.JsonOpts);
            return (true, null, data);
        }

        public async Task<(bool Ok, string? Error, Parametro? Data)> GetByIdAsync(string id, string token)
        {
            using var request = NuevoRequest(HttpMethod.Get, $"parametro/{id}", token);
            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return (false, await ApiHelper.LeerErrorAsync(response), null);

            var data = await response.Content.ReadFromJsonAsync<Parametro>(ApiHelper.JsonOpts);
            return (true, null, data);
        }

        public async Task<(bool Ok, string? Error)> CreateAsync(string id, string valor, string token)
        {
            using var request = NuevoRequest(HttpMethod.Post, "parametro", token, new { id, valor });
            var response = await _http.SendAsync(request);

            return response.IsSuccessStatusCode
                ? (true, null)
                : (false, await ApiHelper.LeerErrorAsync(response));
        }

        public async Task<(bool Ok, string? Error)> UpdateAsync(string id, string valor, string token)
        {
            using var request = NuevoRequest(HttpMethod.Put, $"parametro/{id}", token, new { id, valor });
            var response = await _http.SendAsync(request);

            return response.IsSuccessStatusCode
                ? (true, null)
                : (false, await ApiHelper.LeerErrorAsync(response));
        }

        public async Task<(bool Ok, string? Error)> DeleteAsync(string id, string token)
        {
            using var request = NuevoRequest(HttpMethod.Delete, $"parametro/{id}", token);
            var response = await _http.SendAsync(request);

            return response.IsSuccessStatusCode
                ? (true, null)
                : (false, await ApiHelper.LeerErrorAsync(response));
        }
    }
}
