using System.Net;
using UsuariosSRV4.DTOs;

namespace UsuariosSRV4.Services
{
    public interface IAreaApiClient
    {
        Task<List<AreaDto>> GetAllAsync(CancellationToken ct = default);
        Task<AreaDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<(bool ok, HttpStatusCode status, string? message)> CreateAsync(AreaCreateDto dto, CancellationToken ct = default);
        Task<(bool ok, HttpStatusCode status, string? message)> UpdateAsync(int id, AreaUpdateDto dto, CancellationToken ct = default);
        Task<(bool ok, HttpStatusCode status, string? message)> DeleteAsync(int id, CancellationToken ct = default);
    }
}