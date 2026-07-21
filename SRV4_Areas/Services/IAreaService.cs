using AreasSRV7.DTOs;
using SRV4_Areas.DTOs;

namespace SRV4_Areas.Services;

public interface IAreaService
{
    Task<IEnumerable<AreaDto>> GetAll();
    Task<AreaDto?> GetById(int id);  // ✅ Agregar este método
    Task<(bool success, string message, int? id)> Create(CreateAreaRequest request);
    Task<(bool success, string message)> Update(UpdateAreaRequest request);
    Task<(bool success, string message)> Delete(int id);
}