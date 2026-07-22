using System.Net;
using UsuariosSRV4.DTOs;

namespace UsuariosSRV4.Services
{
    public interface ITipoUsuarioApiClient
    {
        Task<List<TipoUsuarioDto>> GetAllAsync(CancellationToken ct = default);
        Task<TipoUsuarioDto?> GetByIdAsync(int id, CancellationToken ct = default);
    }
}