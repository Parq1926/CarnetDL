using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CarnetDigitalWeb.Services
{
    public class CarnetQRService : ICarnetQRService
    {
        private readonly HttpClient _http;

        public CarnetQRService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("CarnetQR");
        }

        public async Task<(bool Ok, string? Error, string? QrBase64)> ObtenerQRAsync(string identificacion, string token)
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Get, $"usuario/qr?identificacion={Uri.EscapeDataString(identificacion)}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return (false, await ApiHelper.LeerErrorAsync(response), null);

            // SRV14 devuelve el string base64 directo, no un objeto
            var qr = await response.Content.ReadFromJsonAsync<string>(ApiHelper.JsonOpts);
            return (true, null, qr);
        }
    }
}
