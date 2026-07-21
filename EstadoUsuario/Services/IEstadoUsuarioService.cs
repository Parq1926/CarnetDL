namespace SRV12_EstadoUsuario.Services
{
    public interface IEstadoUsuarioService
    {
        Task<int> CambiarEstadoAsync(string identificacion, string codigoEstado);
    }
}
