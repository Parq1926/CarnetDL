using LoginSRV1.DTOs;
using System.Text.Json;

namespace LoginSRV1.Services
{
    public class LoginApiClient : ILoginApiClient
    {
        private readonly HttpClient _httpClient;

        public LoginApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResponseDto?> ValidarCredencialesAsync(LoginRequestDto request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        }

        public async Task<LoginResponseDto?> RefreshTokenAsync(string refreshToken)
        {
            var request = new { RefreshToken = refreshToken };
            var response = await _httpClient.PostAsJsonAsync("api/auth/refresh", request);
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            var request = new { Token = token };
            var response = await _httpClient.PostAsJsonAsync("api/auth/validate", request);
            return response.IsSuccessStatusCode;
        }
    }
}