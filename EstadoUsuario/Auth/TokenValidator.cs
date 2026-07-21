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

            var baseUrl = _configuration["Services:LoginSRV1"] ?? "http://localhost:5001";

            try
            {
                var response = await _httpClient.GetAsync(
                    $"{baseUrl}/api/auth/validate?token={Uri.EscapeDataString(token)}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
