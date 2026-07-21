namespace BitacoraSRV9.Auth;

public interface ITokenValidator
{
    Task<bool> ValidateAsync(string token);
}