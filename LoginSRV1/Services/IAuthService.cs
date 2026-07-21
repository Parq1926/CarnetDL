using LoginSRV1.DTOs;

namespace LoginSRV1.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
        Task<LoginResponseDto> RefreshTokenAsync(string refreshToken);
        Task<bool> LogoutAsync(string refreshToken);
        Task<bool> ValidateTokenAsync(string token);
    }
}