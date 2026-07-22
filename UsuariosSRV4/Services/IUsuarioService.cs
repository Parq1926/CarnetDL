using UsuariosSRV4.DTOs;

namespace UsuariosSRV4.Services
{
    public interface IUsuarioService
    {
        Task<(bool ok, string? error, IEnumerable<UsuarioDto>? data)> GetAllAsync();
        Task<(bool ok, string? error, UsuarioDto? data)> GetByIdAsync(int id);
        Task<(bool ok, string? error, UsuarioDto? data)> CreateAsync(CrearUsuarioDto dto);
        Task<(bool ok, string? error, UsuarioDto? data)> UpdateAsync(int id, ActualizarUsuarioDto dto);
        Task<(bool ok, string? error)> DeleteAsync(int id);
        Task<(bool ok, string? error, ValidarCredencialesResponse? data)> ValidarCredencialesAsync(string email, string password, string? tipo = null);
    }
}