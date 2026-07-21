namespace RolSRV8.Auth;

public interface ITokenValidator
{
    Task<bool> ValidateAsync(string token);
}