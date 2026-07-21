using SRV12_EstadoUsuario.Entities;

namespace SRV12_EstadoUsuario.Services
{
    public interface IEstadoUsuarioService
    {
        // -1 usuario no existe | -2 estado no existe | 0 no se aplico | 1 correcto
        Task<(int Resultado, UsuarioEstado? Usuario)> CambiarEstadoAsync(int usuarioId, string codigoEstado);
    }
}
