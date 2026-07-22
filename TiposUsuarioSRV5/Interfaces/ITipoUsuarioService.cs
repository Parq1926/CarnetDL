using TiposUsuarioSRV5.DTOs;

namespace TiposUsuarioSRV5.Interfaces
{
    public interface ITipoUsuarioService
    {
        Task<IEnumerable<TipoUsuarioDto>> GetAllAsync();
        Task<TipoUsuarioDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(TipoUsuarioCreateDto dto);
        Task<int> UpdateAsync(int id, TipoUsuarioUpdateDto dto);
        Task<int> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByNameAsync(string nombre, int? excludeId = null);
    }
}
