using System.Net;
using UsuariosSRV4.DTOs;

namespace UsuariosSRV4.Services
{
    public interface IUsuarioApiClient
    {
        Task<List<UsuarioDto>> GetAllAsync(CancellationToken ct = default);
        Task<UsuarioDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<(bool ok, HttpStatusCode status, string? message, UsuarioDto? data)> CreateAsync(CrearUsuarioDto dto, CancellationToken ct = default);
        Task<(bool ok, HttpStatusCode status, string? message, UsuarioDto? data)> UpdateAsync(int id, ActualizarUsuarioDto dto, CancellationToken ct = default);
        Task<(bool ok, HttpStatusCode status, string? message)> DeleteAsync(int id, CancellationToken ct = default);
        Task<bool> DesbloquearUsuarioAsync(int id, CancellationToken ct = default);
        Task<List<UsuarioDto>> GetByFilterAsync(FiltroUsuarioDto filtro, CancellationToken ct = default);
        Task<ValidarCredencialesResponse?> ValidarCredencialesAsync(string email, string password, string tipo, CancellationToken ct = default);
    }
}