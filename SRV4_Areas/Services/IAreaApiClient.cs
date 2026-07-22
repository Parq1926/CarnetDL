using System.Net;
using AreasSRV7.DTOs;

namespace AreasSRV7.Services
{
    public interface IAreaApiClient
    {
        Task<List<AreaDto>> GetAllAsync(CancellationToken ct = default);
        Task<AreaDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<(bool ok, HttpStatusCode status, string? message, AreaDto? data)> CreateAsync(AreaCreateDto dto, CancellationToken ct = default);
        Task<(bool ok, HttpStatusCode status, string? message, AreaDto? data)> UpdateAsync(int id, AreaUpdateDto dto, CancellationToken ct = default);
        Task<(bool ok, HttpStatusCode status, string? message)> DeleteAsync(int id, CancellationToken ct = default);
    }
}