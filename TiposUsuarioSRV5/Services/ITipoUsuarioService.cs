using TiposUsuarioSRV5.DTOs;

namespace TiposUsuarioSRV5.Services
{
    public interface ITipoUsuarioService
    {
        Task<IEnumerable<TipoUsuarioDto>> GetAllAsync();
        Task<TipoUsuarioDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(TipoUsuarioCreateDto dto);
        Task<int> UpdateAsync(TipoUsuarioUpdateDto dto);
        Task<int> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByNameAsync(string nombre, int? excludeId = null);
    }
}