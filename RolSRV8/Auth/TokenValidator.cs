using System.Net.Http.Headers;

namespace RolSRV8.Auth
{
    public class TokenValidator : ITokenValidator
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public TokenValidator(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<bool> ValidateAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            var loginUrl = _configuration["Services:LoginSRV1"];

            if (string.IsNullOrWhiteSpace(loginUrl))
                throw new InvalidOperationException(
                    "No está configurada la URL de LoginSRV1.");

            try
            {
                var url = $"{loginUrl}/api/Auth/validate";

                var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    url);

                request.Headers.Add(
                    "Authorization",
                    $"Bearer {token}");

                Console.WriteLine("Validando token en:");
                Console.WriteLine(url);
                Console.WriteLine($"Bearer {token}");

                var response = await _httpClient.SendAsync(request);

                Console.WriteLine($"Respuesta LoginSRV1: {response.StatusCode}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}