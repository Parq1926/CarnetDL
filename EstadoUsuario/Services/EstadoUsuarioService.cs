using SRV12_EstadoUsuario.Entities;
using SRV12_EstadoUsuario.Repository;

namespace SRV12_EstadoUsuario.Services
{
    public class EstadoUsuarioService : IEstadoUsuarioService
    {
        private readonly EstadoUsuarioRepository _repository;
        public EstadoUsuarioService(EstadoUsuarioRepository repository) { _repository = repository; }

        public async Task<(int, UsuarioEstado?)> CambiarEstadoAsync(int usuarioId, string codigoEstado)
        {
            if (!await _repository.ExisteUsuarioAsync(usuarioId))
                return (-1, null);

            var estado = await _repository.GetEstadoAsync(codigoEstado);
            if (estado is null) return (-2, null);

            var filas = await _repository.CambiarEstadoAsync(usuarioId, estado.Id);
            if (filas <= 0) return (0, null);

            return (1, await _repository.GetUsuarioEstadoAsync(usuarioId));
        }
    }
}
