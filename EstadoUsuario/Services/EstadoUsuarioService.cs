using SRV12_EstadoUsuario.Repository;

namespace SRV12_EstadoUsuario.Services
{
    public class EstadoUsuarioService : IEstadoUsuarioService
    {
        private readonly EstadoUsuarioRepository _repository;
        public EstadoUsuarioService(EstadoUsuarioRepository repository) { _repository = repository; }

        public async Task<int> CambiarEstadoAsync(string identificacion, string codigoEstado)
        {
            var estado = await _repository.GetEstadoByCodigoAsync(codigoEstado);
            if (estado is null) return -1;
            return await _repository.UpsertEstadoUsuarioAsync(identificacion, estado.Id);
        }
    }
}
