using CarnetDigitalWeb.Models;

namespace CarnetDigitalWeb.Services
{
    public interface ITipoIdentificacionService
    {
        Task<List<TipoIdentificacion>> GetAllAsync();
        Task<TipoIdentificacion?> GetByIdAsync(int id);
        Task<bool> CreateAsync(TipoIdentificacionCreateDto dto);
        Task<bool> UpdateAsync(TipoIdentificacionUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}