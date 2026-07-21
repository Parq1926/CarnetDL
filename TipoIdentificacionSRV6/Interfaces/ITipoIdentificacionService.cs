using TipoIdentificacionSRV6.DTOs;

namespace TipoIdentificacionSRV6.Interfaces
{
    public interface ITipoIdentificacionService
    {
        Task<IEnumerable<TipoIdentificacionDto>> GetAllAsync();
        Task<TipoIdentificacionDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(TipoIdentificacionCreateDto dto);
        Task<int> UpdateAsync(int id, TipoIdentificacionUpdateDto dto);
        Task<int> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByNameAsync(string nombre, int? excludeId = null);
    }
}