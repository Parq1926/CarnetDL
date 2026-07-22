namespace SRV12_EstadoUsuario.Auth
{
    public interface ITokenValidator
    {
        Task<bool> ValidateAsync(string token);
    }

    public class TokenValidator : ITokenValidator
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public TokenValidator(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<bool> ValidateAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            var baseUrl = (_configuration["Services:LoginSRV1"] ?? "http://localhost:5019").TrimEnd('/');

            try
            {
                // El endpoint GET /api/Auth/validate del SRV1 lee el token del header
                // Authorization: Bearer <token>
                using var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    $"{baseUrl}/api/Auth/validate?token={Uri.EscapeDataString(token)}");

                request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");

                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
