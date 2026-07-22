using System.Net;
using UsuariosSRV4.DTOs;

namespace UsuariosSRV4.Services
{
    public interface ICarreraApiClient
    {
        Task<List<CarreraDto>> GetAllAsync(CancellationToken ct = default);
        Task<CarreraDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<(bool ok, HttpStatusCode status, string? message)> CreateAsync(CarreraCreateDto dto, CancellationToken ct = default);
        Task<(bool ok, HttpStatusCode status, string? message)> UpdateAsync(int id, CarreraUpdateDto dto, CancellationToken ct = default);
        Task<(bool ok, HttpStatusCode status, string? message)> DeleteAsync(int id, CancellationToken ct = default);
    }
}