using TiposUsuarioSRV5.DTOs;

namespace TiposUsuarioSRV5.Services
{
    public class TipoUsuarioService : ITipoUsuarioService
    {
        private readonly ITipoUsuarioApiClient _apiClient;

        public TipoUsuarioService(ITipoUsuarioApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IEnumerable<TipoUsuarioDto>> GetAllAsync()
        {
            return await _apiClient.GetAllAsync();
        }

        public async Task<TipoUsuarioDto?> GetByIdAsync(int id)
        {
            return await _apiClient.GetByIdAsync(id);
        }

        public async Task<int> CreateAsync(TipoUsuarioCreateDto dto)
        {
            var (ok, status, message) = await _apiClient.CreateAsync(dto);
            if (!ok)
            {
                throw new Exception(message ?? "Error al crear el tipo de usuario");
            }
            var all = await _apiClient.GetAllAsync();
            return all.OrderByDescending(x => x.Id).FirstOrDefault()?.Id ?? 0;
        }

        public async Task<int> UpdateAsync(TipoUsuarioUpdateDto dto)
        {
            var (ok, status, message) = await _apiClient.UpdateAsync(dto.Id, dto);
            if (!ok)
            {
                throw new Exception(message ?? "Error al actualizar el tipo de usuario");
            }
            return dto.Id;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var (ok, status, message) = await _apiClient.DeleteAsync(id);
            if (!ok)
            {
                throw new Exception(message ?? "Error al eliminar el tipo de usuario");
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