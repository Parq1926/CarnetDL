using CarnetDigitalWeb.Models;

namespace CarnetDigitalWeb.Services
{
    public interface IEstadoUsuarioService
    {
        Task<(bool Ok, string? Error, UsuarioEstado? Data)> CambiarEstadoAsync(
            int usuarioId, string estado, string token);
    }
}
