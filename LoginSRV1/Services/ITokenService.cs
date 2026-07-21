using LoginSRV1.Entities;

namespace LoginSRV1.Services
{
    public interface ITokenService
    {
        string GenerateToken(Usuario usuario);
        bool ValidateToken(string token);
    }
}