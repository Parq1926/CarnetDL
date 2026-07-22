using System.Net;
using UsuariosSRV4.DTOs;

namespace UsuariosSRV4.Services
{
    public interface ITipoIdentificacionApiClient
    {
        Task<List<TipoIdentificacionDto>> GetAllAsync(CancellationToken ct = default);
        Task<TipoIdentificacionDto?> GetByIdAsync(int id, CancellationToken ct = default);
    }
}