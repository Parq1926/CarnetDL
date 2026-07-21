using UsuariosSRV4.DTOs;

namespace UsuariosSRV4.Services
{
    public interface IUsuarioService
    {
        // ✅ Validar credenciales (para LoginSRV1)
        Task<ValidarCredencialesResponse?> ValidarCredencialesAsync(string email, string password, string tipo);

        Task<IEnumerable<UsuarioDto>> GetAllAsync();
        Task<UsuarioDto?> GetByIdAsync(int id);
        Task<(bool ok, string? error, UsuarioDto? data)> CreateAsync(CrearUsuarioDto dto);
        Task<(bool ok, string? error, UsuarioDto? data)> UpdateAsync(int id, ActualizarUsuarioDto dto);
        Task<bool> DesbloquearUsuarioAsync(int id);
        Task<(bool ok, string? error)> DeleteAsync(int id);
        Task<IEnumerable<UsuarioDto>> GetByFilterAsync(FiltroUsuarioDto filtro);
    }
}