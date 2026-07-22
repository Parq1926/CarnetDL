using CarnetDigitalWeb.Models;

namespace CarnetDigitalWeb.Services
{
    public interface IFotografiaService
    {
        Task<(bool Ok, string? Error, string? FotoBase64)> ObtenerAsync(int usuarioId, string token);

        Task<(bool Ok, string? Error, FotografiaUsuario? Data)> ActualizarAsync(
            int usuarioId, string fotoBase64, string token);

        Task<(bool Ok, string? Error)> EliminarAsync(int usuarioId, string token);
    }
}
