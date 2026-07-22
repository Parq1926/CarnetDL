using CarnetDigitalWeb.Models;

namespace CarnetDigitalWeb.Services
{
    public interface IParametroService
    {
        Task<(bool Ok, string? Error, List<Parametro>? Data)> GetAllAsync(string token);
        Task<(bool Ok, string? Error, Parametro? Data)> GetByIdAsync(string id, string token);
        Task<(bool Ok, string? Error)> CreateAsync(string id, string valor, string token);
        Task<(bool Ok, string? Error)> UpdateAsync(string id, string valor, string token);
        Task<(bool Ok, string? Error)> DeleteAsync(string id, string token);
    }
}
