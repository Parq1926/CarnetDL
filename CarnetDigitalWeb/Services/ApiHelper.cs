using System.Net.Http.Json;
using System.Text.Json;

namespace CarnetDigitalWeb.Services
{
    // Los microservicios usan camelCase por defecto (System.Text.Json en minimal APIs)
    public static class ApiHelper
    {
        public static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

        // Los microservicios devuelven { "message": "..." } cuando hay error
        public static async Task<string> LeerErrorAsync(HttpResponseMessage resp)
        {
            try
            {
                var doc = await resp.Content.ReadFromJsonAsync<JsonElement>();
                if (doc.TryGetProperty("message", out var m))
                    return m.GetString() ?? $"Error {(int)resp.StatusCode}";
            }
            catch
            {
                // el cuerpo no era JSON, se usa el codigo de estado
            }
            return $"Error {(int)resp.StatusCode}";
        }
    }
}
