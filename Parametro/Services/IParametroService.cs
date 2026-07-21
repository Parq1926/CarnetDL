using SRV15_Parametro.Entities;

namespace SRV15_Parametro.Services
{
    public interface IParametroService
    {
        Task<IEnumerable<Parametro>> GetAllAsync();
        Task<Parametro?> GetByIdAsync(string id);
        Task<(bool ok, string error)> CreateAsync(ParametroRequest request);
        Task<(bool ok, string error)> UpdateAsync(string id, ParametroRequest request);
        Task<(bool ok, string error)> DeleteAsync(string id);
    }
}
