using SRV11_AutoRegistro.Entities;

namespace SRV11_AutoRegistro.Services
{
    public interface IUsuarioService
    {
        Task<(bool ok, string error, Usuario? usuarioCreado)> RegistrarAsync(Usuario usuario);

        Task<(bool ok, string error)> ConfirmarCuentaAsync(string token);
    }
}