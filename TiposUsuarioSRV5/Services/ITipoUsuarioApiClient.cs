using System.Net;
using TiposUsuarioSRV5.DTOs;

namespace TiposUsuarioSRV5.Services
{
    public interface ITipoUsuarioApiClient
    {
        Task<List<TipoUsuarioDto>> GetAllAsync(CancellationToken ct = default);
        Task<TipoUsuarioDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<(bool ok, HttpStatusCode status, string? message)> CreateAsync(TipoUsuarioCreateDto dto, CancellationToken ct = default);
        Task<(bool ok, HttpStatusCode status, string? message)> UpdateAsync(int id, TipoUsuarioUpdateDto dto, CancellationToken ct = default);
        Task<(bool ok, HttpStatusCode status, string? message)> DeleteAsync(int id, CancellationToken ct = default);
        Task<bool> ExistsAsync(int id, CancellationToken ct = default);
        Task<bool> ExistsByNameAsync(string nombre, int? excludeId = null, CancellationToken ct = default);
    }
}