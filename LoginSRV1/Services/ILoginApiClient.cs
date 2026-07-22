using LoginSRV1.DTOs;

namespace LoginSRV1.Services
{
    public interface ILoginApiClient
    {
        Task<LoginResponseDto?> ValidarCredencialesAsync(LoginRequestDto request);
        Task<LoginResponseDto?> RefreshTokenAsync(string refreshToken);
        Task<bool> ValidateTokenAsync(string token);
    }
}