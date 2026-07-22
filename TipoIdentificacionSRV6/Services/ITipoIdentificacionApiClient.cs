using System.Net;
using TipoIdentificacionSRV6.DTOs;

namespace TipoIdentificacionSRV6.Services
{
    public interface ITipoIdentificacionApiClient
    {
        Task<List<TipoIdentificacionDto>> GetAllAsync(CancellationToken ct = default);
        Task<TipoIdentificacionDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<(bool ok, HttpStatusCode status, string? message)> CreateAsync(TipoIdentificacionCreateDto dto, CancellationToken ct = default);
        Task<(bool ok, HttpStatusCode status, string? message)> UpdateAsync(int id, TipoIdentificacionUpdateDto dto, CancellationToken ct = default);
        Task<(bool ok, HttpStatusCode status, string? message)> DeleteAsync(int id, CancellationToken ct = default);
        Task<bool> ExistsAsync(int id, CancellationToken ct = default);
        Task<bool> ExistsByNameAsync(string nombre, int? excludeId = null, CancellationToken ct = default);
    }
}