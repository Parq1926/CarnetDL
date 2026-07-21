using TipoIdentificacionSRV6.DTOs;

namespace TipoIdentificacionSRV6.Services
{
    public class TipoIdentificacionService : ITipoIdentificacionService
    {
        private readonly ITipoIdentificacionApiClient _apiClient;

        public TipoIdentificacionService(ITipoIdentificacionApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IEnumerable<TipoIdentificacionDto>> GetAllAsync()
        {
            return await _apiClient.GetAllAsync();
        }

        public async Task<TipoIdentificacionDto?> GetByIdAsync(int id)
        {
            return await _apiClient.GetByIdAsync(id);
        }

        public async Task<int> CreateAsync(TipoIdentificacionCreateDto dto)
        {
            var (ok, status, message) = await _apiClient.CreateAsync(dto);
            if (!ok)
            {
                throw new Exception(message ?? "Error al crear el tipo de identificacion");
            }
            var all = await _apiClient.GetAllAsync();
            return all.OrderByDescending(x => x.Id).FirstOrDefault()?.Id ?? 0;
        }

        public async Task<int> UpdateAsync(TipoIdentificacionUpdateDto dto)
        {
            var (ok, status, message) = await _apiClient.UpdateAsync(dto.Id, dto);
            if (!ok)
            {
                throw new Exception(message ?? "Error al actualizar el tipo de identificacion");
            }
            return dto.Id;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var (ok, status, message) = await _apiClient.DeleteAsync(id);
            if (!ok)
            {
                throw new Exception(message ?? "Error al eliminar el tipo de identificacion");
            }
            return id;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _apiClient.ExistsAsync(id);
        }

        public async Task<bool> ExistsByNameAsync(string nombre, int? excludeId = null)
        {
            return await _apiClient.ExistsByNameAsync(nombre, excludeId);
        }
    }
}