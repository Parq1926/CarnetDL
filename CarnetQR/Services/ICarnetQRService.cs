namespace SRV14_CarnetQR.Services
{
    public interface ICarnetQRService
    {
        Task<string?> GenerarQRAsync(string identificacion);
    }
}
