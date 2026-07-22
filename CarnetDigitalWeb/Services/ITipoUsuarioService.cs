using CarnetDigitalWeb.Models;

namespace CarnetDigitalWeb.Services
{
    public interface ITipoUsuarioService
    {
        Task<List<TipoUsuario>> GetAllAsync();
        Task<TipoUsuario?> GetByIdAsync(int id);
        Task<bool> CreateAsync(TipoUsuarioCreateDto dto);
        Task<bool> UpdateAsync(TipoUsuarioUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}