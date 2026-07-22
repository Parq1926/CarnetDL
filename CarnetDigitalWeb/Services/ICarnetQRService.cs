namespace CarnetDigitalWeb.Services
{
    public interface ICarnetQRService
    {
        Task<(bool Ok, string? Error, string? QrBase64)> ObtenerQRAsync(string identificacion, string token);
    }
}
